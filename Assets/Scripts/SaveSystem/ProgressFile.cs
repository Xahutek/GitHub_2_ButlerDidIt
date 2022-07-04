using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calendar;

[System.Serializable]
public class ProgressFile
{
    public bool Fresh;

    //Difficulty
    public Difficulty difficulty;

    //Time
    public float currentHour;

    //Player
    public float[] playerPosition;

    //Intermissions
    public List<bool> seenIntermission;

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
        public List<int> 
            chessDataSlots,
            chessDataFigures;
        

        public Minigames_ProgressFile()
        {
            calendar = new List<bool>();
            for (int i = 0; i < 30; i++)
            {
                if (i < 13) calendar.Add(true);
                else calendar.Add(false);
            }

            chessDataSlots = new List<int>();
            chessDataFigures = new List<int>();
        }

        public void Save()
        {
            calendar = MinigameData.calendar;

            chessDataSlots = new List<int>();
            chessDataFigures = new List<int>();
            
            foreach (Vector2Int v in MinigameData.slotFigures)
            {
                chessDataSlots.Add(v.x);
                chessDataFigures.Add(v.y);
            }
        }

        public void Load()
        {
            MinigameData.calendar = calendar;

            MinigameData.slotFigures = new List<Vector2Int>();
            for (int i = 0; i < chessDataSlots.Count; i++)
            {
                MinigameData.slotFigures.Add(new Vector2Int(chessDataSlots[i], chessDataFigures[i]));
            }
        }
    }
    public Minigames_ProgressFile minigames;

    public ProgressFile() //New Game
    {
        difficulty= GameLoadData.difficulty;
        Debug.Log(difficulty);

        currentHour = 0;

        playerPosition = new float[2] { 28.5f, 34f };

        characters = new List<Character_ProgressFile>();
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            characters.Add(new Character_ProgressFile(c));
        }

        clues = new List<Clue_ProgressFile>();

        minigames = new Minigames_ProgressFile();

        seenIntermission = new List<bool>();
        for (int i = 0; i < EventManager.main.allIntermissions.Length; i++)
        {
            seenIntermission.Add(false);
        }
    }

    public void Save()
    {
        difficulty = GameLoadData.difficulty;
        Debug.Log(difficulty);

        currentHour = Clock.Hour;

        Vector2 playerPos = PlayerController.main.position;
        playerPosition = new float[2] { playerPos.x, playerPos.y };

        characters = new List<Character_ProgressFile>();
        foreach (Character c in System.Enum.GetValues(typeof(Character)))
        {
            characters.Add(new Character_ProgressFile(c.Profile()));
        }

        //Intermissions
        seenIntermission = new List<bool>();
        for (int i = 0; i < EventManager.main.allIntermissions.Length; i++)
        {
           seenIntermission.Add(EventManager.main.allIntermissions[i].passed);
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
        GameLoadData.difficulty=difficulty;
        Debug.Log(difficulty);

        //Time
        Debug.Log("Saved Time is"+currentHour);
        Clock.SetTime(currentHour);

        //Player Position
        PlayerController.main.position = playerPosition == null || playerPosition.Length != 2 ? new Vector2(28.5f, 34f) : new Vector2(playerPosition[0], playerPosition[1]);

        //Intermissions
        for(int i = 0; i< EventManager.main.allIntermissions.Length; i++)
        {
            EventManager.main.allIntermissions[i].passed = seenIntermission[i];
        }
        EventManager.main.saveFileLoaded = true;

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
