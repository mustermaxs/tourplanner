namespace Tourplanner.Services.Search
{

    public class FuzzyMatcher
    {
        private int stringLengthWeight;
        private int sensitivity;
        private int costInsertion { get; set; }
        private int costSubstitution { get; set; }
        private int costDeletion { get; set; }
        private int minStringLengthToApplyWeight { get; set; }

        public FuzzyMatcher()
        {
            this.costInsertion = 1;
            this.costSubstitution = 1;
            this.costDeletion = 1;
            this.stringLengthWeight = 1;
            this.minStringLengthToApplyWeight = 6;
            this.sensitivity = 1;
        }

        private int[,] InitMatrix(int lengthStr1, int lengthStr2)
        {
            int[,] arr = new int[lengthStr1 + 1, lengthStr2 + 1];

            for (int i = 0; i <= lengthStr1; i++)
            {
                arr[i, 0] = i;
            }

            for (int j = 0; j <= lengthStr2; j++)
            {
                arr[0, j] = j;
            }

            return arr;
        }

        public int GetEditDistance(string string1, string string2)
        {
            int lengthStr1 = string1.Length;
            int lengthStr2 = string2.Length;

            if (lengthStr1 == 0)
            {
                return lengthStr2;
            }

            if (lengthStr2 == 0)
            {
                return lengthStr1;
            }

            int[,] arr = InitMatrix(lengthStr1, lengthStr2);

            for (int i = 1; i <= lengthStr1; i++)
            {
                for (int j = 1; j <= lengthStr2; j++)
                {
                    if (string1[i - 1] == string2[j - 1])
                    {
                        arr[i, j] = arr[i - 1, j - 1];
                    }
                    else
                    {
                        arr[i, j] = Math.Min(
                            arr[i - 1, j - 1],
                            Math.Min(arr[i, j - 1], arr[i - 1, j])
                        ) + 1;
                    }
                }
            }

            int distance = arr[lengthStr1, lengthStr2];

            return distance;
        }
    }
}