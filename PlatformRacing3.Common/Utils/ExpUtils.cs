using System.Collections.Immutable;

namespace PlatformRacing3.Common.Utils;

public static class ExpUtils
{
	public const ulong EXP_FOR_FINISHING = 5;

	public const ulong EXP_BASE_FOR_DEFEATING_PLAYER = 5;
	public const double EXP_SCALE_FPR_DEFEATING_PLAYER = 1;

	public const double REQUIRED_PLAYTIME = 120;
	public const double PLAYTIME_MAX_SCALE = 1;

	public const double REQUIRED_KEY_PRESSES = 60;
	public const double KEY_PRESSES_MAX_SCALE = 1;

	private static readonly ImmutableArray<ulong> ExperienceRequirements = ExpUtils.GenerateExperienceRequirements();
	private static readonly ImmutableArray<ulong> TotalExperienceRequirements = ExpUtils.GenerateTotalExperienceRequirements();

	private static ImmutableArray<ulong> GenerateExperienceRequirements()
	{
		static ulong GetExperienceRequirement(uint rank) => rank == 0 ? 1 : checked((ulong)Math.Floor(30 * Math.Pow(1.25, rank - 1)));

		ImmutableArray<ulong>.Builder builder = ImmutableArray.CreateBuilder<ulong>(184 + 1); //184 is the limit until we overflow

		for (uint rank = 0; rank <= 184; rank++)
		{
			builder.Add(GetExperienceRequirement(rank));
		}

		return builder.ToImmutable();
	}

	private static ImmutableArray<ulong> GenerateTotalExperienceRequirements()
	{
		ImmutableArray<ulong>.Builder builder = ImmutableArray.CreateBuilder<ulong>(178 + 1); //178 is the limit until we overflow

		ulong experience = 0;
		for (uint rank = 0; rank <= 177; rank++)
		{
			builder.Add(experience);

			checked
			{
				experience += ExpUtils.GetNextRankExpRequirement(rank);
			}
		}

		builder.Add(experience);

		return builder.ToImmutable();
	}

	public static (uint Rank, ulong Exp) GetRankAndExpFromTotalExp(ulong totalExp)
	{
		int rank = ExpUtils.TotalExperienceRequirements.BinarySearch(totalExp);
		if (rank < 0)
		{
			rank = ~rank - 1;
		}

		return ((uint)rank, totalExp - ExpUtils.TotalExperienceRequirements[rank]);
	}

	public static ulong GetNextRankExpRequirement(uint rank) => ExpUtils.ExperienceRequirements[(int)rank];

	public static ulong GetExpEarnedForFinishing(double finishTime) => (ulong)Math.Round(ExpUtils.EXP_FOR_FINISHING * ExpUtils.GetPlaytimeMultiplayer(finishTime));
	public static ulong GetExpForDefeatingPlayer(uint playerRank) => (ulong)Math.Round(ExpUtils.EXP_BASE_FOR_DEFEATING_PLAYER + (playerRank * ExpUtils.EXP_SCALE_FPR_DEFEATING_PLAYER));

	public static double GetPlaytimeMultiplayer(double playtime) => Math.Min(playtime / ExpUtils.REQUIRED_PLAYTIME, ExpUtils.PLAYTIME_MAX_SCALE);
	public static double GetKeyPressMultiplayer(uint keyPresses) => Math.Min(keyPresses / ExpUtils.REQUIRED_KEY_PRESSES, ExpUtils.KEY_PRESSES_MAX_SCALE);
}
