using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Platform_Racing_3_Common.Utils
{
    public static class ExpUtils
    {
        public const ulong EXP_FOR_FINISHING = 5;

        public const ulong EXP_BASE_FOR_DEFEATING_PLAYER = 5;
        public const double EXP_SCALE_FPR_DEFEATING_PLAYER = 1;

        public const double REQUIRED_PLAYTIME = 120;
        public const double PLAYTIME_MAX_SCALE = 1;

        public const double REQUIRED_KEY_PRESSES = 60;
        public const double KEY_PRESSES_MAX_SCALE = 1;

        public static (uint rank, ulong exp) GetRankAndExpFromTotalExp(ulong totalExp)
        {
            uint currentRank = 0;
            while (totalExp > 0)
            {
                ulong expNeeded = ExpUtils.GetNextRankExpRequirement(currentRank);

                try
                {
                    checked
                    {
                        totalExp -= expNeeded;
                        currentRank++;

                        if (totalExp == 0)
                        {
                            break;
                        }
                    }
                }
                catch(OverflowException)
                {
                    break;
                }
            }

            return (currentRank, totalExp);
        }

        public static ulong GetNextRankExpRequirement(uint rank) => rank == 0 ? 1 : checked((ulong)Math.Floor(30 * Math.Pow(1.25, rank - 1)));

        public static ulong GetExpEarnedForFinishing(double finishTime) => (ulong)Math.Round(ExpUtils.EXP_FOR_FINISHING * ExpUtils.GetPlaytimeMultiplayer(finishTime));
        public static ulong GetExpForDefeatingPlayer(uint playerRank) => (ulong)Math.Round(ExpUtils.EXP_BASE_FOR_DEFEATING_PLAYER + (playerRank * ExpUtils.EXP_SCALE_FPR_DEFEATING_PLAYER));

        public static double GetPlaytimeMultiplayer(double playtime) => Math.Min(playtime / ExpUtils.REQUIRED_PLAYTIME, ExpUtils.PLAYTIME_MAX_SCALE);
        public static double GetKeyPressMultiplayer(uint keyPresses) => Math.Min(keyPresses / ExpUtils.REQUIRED_KEY_PRESSES, ExpUtils.KEY_PRESSES_MAX_SCALE);
    }
}
