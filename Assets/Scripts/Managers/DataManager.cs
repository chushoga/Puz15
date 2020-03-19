﻿public static class DataManager
{
    private static int puzzleSize; // The puzzle size.
    private static int imageIndex; // The chosen Image index

    // Puzzle Size - from 3 to 8
    public static int PuzzleSize
    {
        get
        {
            return puzzleSize;
        }
        set
        {
            puzzleSize = value;
        }
            
    }

    // Image Index
    public static int ImageIndex
    {
        get
        {
            return imageIndex;
        }
        set
        {
            imageIndex = value;
        }
    }
    

}
