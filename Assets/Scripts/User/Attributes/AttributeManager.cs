using Assets.Scripts.Misc.ObjectManager;

namespace Assets.Scripts.User.Attributes
{
    public static class AttributeManager
    {
        private static CharacterEntity _characterEntity;
        private static CharacterEntity CharacterEntity => _characterEntity ?? (_characterEntity = DI.Fetch<CharacterEntity>());

        private static AttributeData _backupAttributeData;
        private static AttributeData Attribute => CharacterEntity.Attributes;

        private static int _backupPoints;
        public static int Points
        {
            get => Attribute.Points;
            private set => Attribute.Points = value;
        }

        public static bool IsModified { get; private set; }

        public static int Level => CharacterEntity.Stats.CurrentLevel;

        public static int GetPointsFor(int attributeId) =>
            _backupAttributeData[attributeId];

        public static void Reset()
        {
            _characterEntity = null;
        }

        public static void AddPointTo(int attributeId)
        {
            if (Points <= 0)
                return;

            _backupAttributeData[attributeId]++;
            IsModified = true;
            Points--;
        }

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

        public static void Save()
        {
            AttributeData.MoveData(Attribute, _backupAttributeData);
            Reload();
        }

        public static void Cancel()
        {
            Points = _backupPoints;
            Reload();
        }

        public static void Reload()
        {
            IsModified = false;
            _backupPoints = Points;
            _backupAttributeData = new AttributeData();
            AttributeData.MoveData(_backupAttributeData, Attribute);
        }

    }
}
