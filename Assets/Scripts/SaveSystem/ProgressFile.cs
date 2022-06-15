using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calendar;

[System.Serializable]
public class ProgressFile
{
    public bool Fresh;
    //Time
    public float currentHour, currentMinute;

    //Player
    public float[] playerPosition;

    //Characters
    [System.Serializable]
    public class Character_ProgressFile
    {
        public Character identity;
        public bool knownToPlayer;
        public List<Dialogue_ProgressFile> dialogues;
        [System.Serializable]
        public class Dialogue_ProgressFile
        {
            public string name;
            public bool seen;

            public Dialogue_ProgressFile(Dialogue d)
            {
                name = d.name;
                seen = d.seen;
            }

            public void Load(Dialogue d)
            {
                d.seen = seen;
            }
        }

        public Character_ProgressFile(CharacterProfile profile)
        {
            identity = profile.identity;
            knownToPlayer = profile.knownToPlayer;

            dialogues = new List<Dialogue_ProgressFile>();
            foreach (Dialogue d in profile.dialogues)
            {
                dialogues.Add(new Dialogue_ProgressFile(d));
            }
        }

        public Character_ProgressFile(Character i)//New
        {
            identity = i;
            knownToPlayer = identity == Character.Butler || identity == Character.Lord || identity == Character.Gardener;
            dialogues = new List<Dialogue_ProgressFile>();
        }

        public void Load()
        {
            CharacterProfile profile = identity.Profile();
            profile.knownToPlayer = knownToPlayer;

            foreach (Dialogue dialogue in profile.dialogues)
            {
                foreach (Dialogue_ProgressFile d in dialogues)
                {
                    if (d.name == dialogue.name)
                        d.Load(dialogue);
                }
            }
        }
    }

    public List<Character_ProgressFile> characters;

    //Clues
    [System.Serializable]
    public class Clue_ProgressFile
    {
        public string name;
        public bool
        ButlerKnows,
        DetectiveKnows,
        ImposterKnows,
        TycoonKnows,
        GeneralKnows,
        GardenerKnows;
        public bool item_givenAway;

        public Clue_ProgressFile(Clue c)
        {
            name = c.name;
            ButlerKnows = c.KnownTo(Character.Butler);
            DetectiveKnows = c.KnownTo(Character.Detective);
            ImposterKnows = c.KnownTo(Character.Imposter);
            TycoonKnows = c.KnownTo(Character.Tycoon);
            GeneralKnows = c.KnownTo(Character.General);
            GardenerKnows = c.KnownTo(Character.Gardener);

            if (c is Item)
                item_givenAway = (c as Item).givenAway;
        }

        public void Load(Clue c)
        {
            bool inEditor = false;
#if UNITY_EDITOR
            inEditor = true;
#endif
            c.AlterKnown(Character.Butler, ButlerKnows, inEditor);
            c.AlterKnown(Character.Detective, DetectiveKnows, inEditor);
            c.AlterKnown(Character.Imposter, ImposterKnows, inEditor);
            c.AlterKnown(Character.Tycoon, TycoonKnows, inEditor);
            c.AlterKnown(Character.General, GeneralKnows, inEditor);
            c.AlterKnown(Character.Gardener, GardenerKnows, inEditor);

            if (c is Item)
                (c as Item).givenAway = item_givenAway;
        }
    }

    public List<Clue_ProgressFile> clues;

    //Minigames
    [System.Serializable]
    public class Minigames_ProgressFile
    {
        //Calendar
        public List<bool> calendar;

        //Chess
        [System.Serializable]
        public class SerializedVector2
        {
            public float x = 0, y = 0;

            public SerializedVector2(float X, float Y)
            {
                x = X;
                y = Y;
            }
            public SerializedVector2(Vector2 vec)
            {
                x = vec.x;
                y = vec.y;
            }
        }
        public SerializedVector2
            blackKing = new SerializedVector2(0, 6),
            whiteKing = new SerializedVector2(7, 7);
        public SerializedVector2[]
            blackfigures = new SerializedVector2[0],
            whiteFigures = new SerializedVector2[0];

        public Minigames_ProgressFile()
        {
            calendar = new List<bool>();
            for (int i = 0; i < 30; i++)
            {
                if (i < 13) calendar.Add(true);
                else calendar.Add(false);
            }

            blackKing = new SerializedVector2(0, 6);
            whiteKing = new SerializedVector2(7, 7);

            blackfigures = new SerializedVector2[0];
            whiteFigures = new SerializedVector2[0];
        }

        public void Save()
        {
            calendar = MinigameData.calendar;

            blackKing = new SerializedVector2(MinigameData.blackKing != null ? MinigameData.blackKing.coordinates : new Vector2(0, 6));
            whiteKing = new SerializedVector2(MinigameData.blackKing != null ? MinigameData.whiteKing.coordinates : new Vector2(7, 7));

            List<SerializedVector2>
                blackList = new List<SerializedVector2>(),
                whiteList = new List<SerializedVector2>();

            if (MinigameData.blackfigures != null) foreach (MinigameData.ChessFigure figure in MinigameData.blackfigures)
                {
                    blackList.Add(new SerializedVector2(figure.coordinates));
                }
            if (MinigameData.blackfigures != null) foreach (MinigameData.ChessFigure figure in MinigameData.whiteFigures)
                {
                    whiteList.Add(new SerializedVector2(figure.coordinates));
                }

            blackfigures = blackList.ToArray();
            whiteFigures = whiteList.ToArray();
        }

        public void Load()
        {
            MinigameData.calendar = calendar;

            MinigameData.blackKing = new MinigameData.ChessFigure(blackKing);
            MinigameData.whiteKing = new MinigameData.ChessFigure(whiteKing);

            List<MinigameData.ChessFigure>
                blackList = new List<MinigameData.ChessFigure>(),
                whiteList = new List<MinigameData.ChessFigure>();

            foreach (SerializedVector2 figure in blackfigures)
            {
                blackList.Add(new MinigameData.ChessFigure(figure));
            }
            foreach (SerializedVector2 figure in whiteFigures)
            {
                whiteList.Add(new MinigameData.ChessFigure(figure));
            }

            MinigameData.blackfigures = blackList.ToArray();
            MinigameData.whiteFigures = whiteList.ToArray();
        }
    }
    public Minigames_ProgressFile minigames;

    public ProgressFile() //New Game
    {
        currentHour = 0;
        currentMinute = 0;

        playerPosition = new float[2] { 28.5f, 34f };

        characters = new List<Character_ProgressFile>();
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            characters.Add(new Character_ProgressFile(c));
        }

        clues = new List<Clue_ProgressFile>();

        minigames = new Minigames_ProgressFile();
    }

    public void Save()
    {
        currentHour = Clock.Hour;
        currentMinute = Clock.Minute;

        Vector2 playerPos = PlayerController.main.position;
        playerPosition = new float[2] { playerPos.x, playerPos.y };

        characters = new List<Character_ProgressFile>();
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            characters.Add(new Character_ProgressFile(c.Profile()));
        }

        clues = new List<Clue_ProgressFile>();

        List<Clue> libraryEntries = new List<Clue>();
        libraryEntries.AddRange(ClueLibrary.main.AllClues);
        libraryEntries.AddRange(ClueLibrary.main.AllItems);

        foreach (Clue c in libraryEntries)
        {
            clues.Add(new Clue_ProgressFile(c));
        }

        if (minigames == null) minigames = new Minigames_ProgressFile();
        minigames.Save();
    }

    public void Load()
    {
        //Time
        Clock.main.currentHour = currentHour;
        Clock.main.currentMinute = currentMinute;

        //Player Position
        PlayerController.main.position = playerPosition == null || playerPosition.Length != 2 ? new Vector2(28.5f, 34f) : new Vector2(playerPosition[0], playerPosition[1]);

        //Characters
        foreach (Character_ProgressFile CP in characters)
        {
            CP.Load();
        }

        //Clues
        List<Clue> libraryEntries = new List<Clue>();
        libraryEntries.AddRange(ClueLibrary.main.AllClues);
        libraryEntries.AddRange(ClueLibrary.main.AllItems);

        foreach (Clue clue in libraryEntries)
        {
            foreach (Clue_ProgressFile c in clues)
            {
                if (c.name == clue.name)
                {
                    c.Load(clue);
                    if (Fresh) clue.FullReset();
                }
            }
        }

        if (minigames == null) minigames = new Minigames_ProgressFile();
        minigames.Load();
    }
}
