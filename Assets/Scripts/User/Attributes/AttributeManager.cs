using Assets.Scripts.Misc.ObjectManager;

namespace Assets.Scripts.User.Attributes
{
    /// <summary>
    /// Middleman class for editing AttributeData from AttributePopupUi
    /// </summary>
    public static class AttributeManager
    {
        private static CharacterEntity _characterEntity;
        private static CharacterEntity CharacterEntity => _characterEntity ?? (_characterEntity = DI.Fetch<CharacterEntity>());

        /// <summary>
        /// Holds edited attribute data
        /// </summary>
        private static AttributeData _backupAttributeData;

        /// <summary>
        /// Holds current attribute data
        /// </summary>
        private static AttributeData Attribute => CharacterEntity.Attributes;

        /// <summary>
        /// Holds edited point count
        /// </summary>
        private static int _backupPoints;

        /// <summary>
        /// Holds current point count
        /// </summary>
        public static int Points
        {
            get => Attribute.Points;
            private set => Attribute.Points = value;
        }

        /// <summary>
        /// Is true when data was edited
        /// </summary>
        public static bool IsModified { get; private set; }

        public static int Level => CharacterEntity.Stats.CurrentLevel;

        public static int GetPointsFor(int attributeId) =>
            _backupAttributeData[attributeId];

        public static void Reset()
        {
            _characterEntity = null;
        }

        /// <summary>
        /// Adds points to backup value
        /// </summary>
        /// <param name="attributeId">Id of attribute to which to add a point</param>
        public static void AddPointTo(int attributeId)
        {
            if (Points <= 0)
                return;

            _backupAttributeData[attributeId]++;
            IsModified = true;
            Points--;
        }

        /// <summary>
        /// Removes point from backup value
        /// </summary>
        /// <param name="attributeId">Id of attribute from whom to remove a point</param>
        public static void RemovePointFrom(int attributeId)
        {
            var attributeValue = _backupAttributeData[attributeId];
            if (attributeValue <= 0)
                return;

            var originalAttributeValue = Attribute[attributeId];
            if(originalAttributeValue >= attributeValue)
                return;

            _backupAttributeData[attributeId]--;
            IsModified = true;
            Points++;
        }

        /// <summary>
        /// Moves edited data - _backupAttributeData to users attributes
        /// </summary>
        public static void Save()
        {
            AttributeData.MoveData(Attribute, _backupAttributeData);
            Reload();
        }

        /// <summary>
        /// Resets backup data
        /// </summary>
        public static void Cancel()
        {
            Points = _backupPoints;
            Reload();
        }

        /// <summary>
        /// Resets backup data
        /// </summary>
        public static void Reload()
        {
            IsModified = false;
            _backupPoints = Points;
            _backupAttributeData = new AttributeData();
            AttributeData.MoveData(_backupAttributeData, Attribute);
        }

    }
}
