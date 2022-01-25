SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[sp_AddQuestPointTransaction]
	@PlayerId varchar(30),
	@QuestPointEarned decimal(18, 2),
	@QuestPointPerMilestone decimal(18, 2),
	@MaxMilestonePerQuest int
AS
BEGIN
	DECLARE @Questid INT = 0
	SELECT @Questid = quest_id FROM QUEST where is_active = 1

	IF @Questid <> 0
	BEGIN
		BEGIN TRAN questPoint

		BEGIN TRY
			INSERT INTO QuestPointTransaction (quest_id, player_id, quest_point_earned, timestamp)
			VALUES (@Questid, @PlayerId, @QuestPointEarned, GETDATE())

			EXEC sp_AddPlayerMilestone @Questid, @PlayerId, @QuestPointEarned, @QuestPointPerMilestone, @MaxMilestonePerQuest

			COMMIT TRAN questPoint
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN questPoint
		END CATCH
	END
END



