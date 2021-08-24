using Microsoft.Extensions.Logging;

namespace PlatformRacing3.Common.Utils
{
	internal static class EventIds
	{

		internal static readonly EventId UserAuthenticationFailed = new(1, nameof(EventIds.UserAuthenticationFailed));
		internal static readonly EventId UserLoadFailed = new(2, nameof(EventIds.UserLoadFailed));
		internal static readonly EventId UserCampaignDataLoadFailed = new(3, nameof(EventIds.UserCampaignDataLoadFailed));
		internal static readonly EventId UserFriendsLoadFailed = new(4, nameof(EventIds.UserFriendsLoadFailed));
		internal static readonly EventId UserIgnoredLoadFailed = new(5, nameof(EventIds.UserIgnoredLoadFailed));
		internal static readonly EventId UserUpdateFailed = new(6, nameof(EventIds.UserUpdateFailed));

		internal static readonly EventId StampLoadFailed = new(7, nameof(EventIds.StampLoadFailed));
		internal static readonly EventId StampSaveFailed = new(8, nameof(EventIds.StampSaveFailed));

		internal static readonly EventId ServerLoadFailed = new(9, nameof(EventIds.ServerLoadFailed));

		internal static readonly EventId PrivateMessageLoadFailed = new(10, nameof(EventIds.PrivateMessageLoadFailed));
		internal static readonly EventId PrivateMessageSendFailed = new(11, nameof(EventIds.PrivateMessageSendFailed));

		internal static readonly EventId LevelLoadFailed = new(12, nameof(EventIds.LevelLoadFailed));
		internal static readonly EventId LevelSaveFailed = new(13, nameof(EventIds.LevelSaveFailed));

		internal static readonly EventId CampaignDataLoadFailed = new(14, nameof(EventIds.CampaignDataLoadFailed));

		internal static readonly EventId BlockLoadFailed = new(15, nameof(EventIds.BlockLoadFailed));
		internal static readonly EventId BlockSaveFailed = new(16, nameof(EventIds.BlockSaveFailed));
	}
}
