using System;
using UnityEngine;

namespace Assets.Scripts.User.Attributes
{
    /// <summary>
    /// Contains information about Entities attributes
    /// </summary>
    public class AttributeData
    {
        private const int ATT_COUNT = 6;

        /// <summary>
        /// Increases hit points AKA Strength
        /// </summary>
        public short Strength { get; set; } = 0;

        /// <summary>
        /// Increases health points AKA HP
        /// </summary>
        public short Vitality { get; set; } = 0;

        /// <summary>
        /// Speed - no implementation yet
        /// </summary>
        public short Agility { get; set; } = 0;

        /// <summary>
        /// Increase magic point AKA Mana
        /// </summary>
        public short Magic { get; set; } = 0;

        /// <summary>
        /// Reduces enemies hit points AKA Defense
        /// </summary>
        public short Resistance { get; set; } = 0;

        /// <summary>
        /// Increases luck, better loot for chests
        /// </summary>
        public short Luck { get; set; } = 0;

        /// <summary>
        /// Points that can be assigned to other attributes
        /// </summary>
        public int Points { get; set; } = 5;

        /// <summary>
        /// Event that is invoked whenever attribute value is updated
        /// </summary>
        public event EventHandler<(int, short)> AttributeUpdate;

        /// <summary>
        /// Helper operation to transform multiple short parameters to
        /// array like access
        /// </summary>
        /// <param name="id">Id of attribute</param>
        /// <returns>Attributes value, on fail - 0</returns>
        public short this[int id]
        {
            get
            {
                switch (id)
                {
                    case 0:
                        return Strength;
                    case 1:
                        return Vitality;
                    case 2:
                        return Agility;
                    case 3:
                        return Magic;
                    case 4:
                        return Resistance;
                    case 5:
                        return Luck;
                    default:
                        return 0;
                }
            }

            set
            {
                short output = 0;
                switch (id)
                {
                    case 0:
                        output = Strength = value;
                        break;
                    case 1:
                        output = Vitality = value;
                        break;
                    case 2:
                        output = Agility = value;
                        break;
                    case 3:
                        output = Magic = value;
                        break;
                    case 4:
                        output = Resistance = value;
                        break;
                    case 5:
                        output = Luck = value;
                        break;
                    default:
                        output = 0;
                        break;
                }
                AttributeUpdate?.Invoke(this, (id, output));
            }
        }

        /// <summary>
        /// Operator overloading for summing two AttributeData objects
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static AttributeData operator +(AttributeData a, AttributeData b)
        {
            for (var i = 0; i < ATT_COUNT; i++)
            {
                a[i] += b[i];
            }

            return a;
        }

        /// <summary>
        /// Operator overloading for multiplying attributes with integer
        /// </summary>
        /// <param name="a">Attribute to be multiplied</param>
        /// <param name="mlt">Amount to which multiply</param>
        /// <returns>Multiplied object</returns>
        public static AttributeData operator *(AttributeData a, short mlt)
        {
            for (var i = 0; i < ATT_COUNT; i++)
            {
                a[i] *= mlt;
            }

            return a;
        }

        /// <summary>
        /// Copies data from B to A
        /// </summary>
        /// <param name="a">To whom copy</param>
        /// <param name="b">From whom to copy</param>
        public static void MoveData(AttributeData a, AttributeData b)
        {
            for (var i = 0; i < ATT_COUNT; i++)
            {
                a[i] = b[i];
            }
        }

        public static AttributeData Copy(AttributeData a)
        {
            var output = new AttributeData();
            MoveData(output, a);
            return output;
        }
    }
}
