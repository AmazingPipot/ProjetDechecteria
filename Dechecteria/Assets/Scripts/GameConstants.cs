namespace Dechecteria
{
    static class GameConstants
    {
	    public enum TILE_TYPE
        {
            OCEAN = 0,
            FOREST = 1,
            CITY = 2,
            MOUNTAIN = 3,
            FACTORY = 4
        }

        public enum GestionRoomType
        {
            ORGA = 0,
            MINERAL = 1,
            METAL = 2,
            PETROL = 3,
            CHIMIC = 4,
            NUCLEAR = 5,
            COMPLEX = 6,
            RECYCLAGE_ORGA = 7,
            RECYCLAGE_MINERAL = 8,
            RECYCLAGE_METAL = 9,
            RECYCLAGE_PETROL = 10,
            RECYCLAGE_CHIMIC = 11,
            RECYCLAGE_NUCLEAR = 12,
            RECYCLAGE_COMPLEX = 13,
            COUNT = 14
        }

        public static float MAX_ENERGY = 1000.0f; //energie max de la colonie
    }
}

