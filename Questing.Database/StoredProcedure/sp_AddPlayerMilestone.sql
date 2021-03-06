SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_AddPlayerMilestone]
	@QuestId int,
	@PlayerId varchar(30),
	@QuestPointEarned decimal(18, 2),
	@QuestPointPerMilestone decimal(18, 2),
	@MaxMilestonePerQuest int
AS
BEGIN
	DECLARE @quest_point_accumulated decimal(18, 2) = 0
	DECLARE @mile_stone_index int = -1
	DECLARE @mile_stone_counter int = 0

	DECLARE @quest_id int = @QuestId
	DECLARE @player_id varchar(30) = @PlayerId
	DECLARE @quest_pt_per_milestone decimal(18, 2) = @QuestPointPerMilestone

	SELECT @quest_point_accumulated = SUM(quest_point_earned) 
	FROM QuestPointTransaction 
	WHERE player_id = @player_id AND quest_id = @quest_id
	GROUP BY player_id, quest_id

	WHILE (ISNULL(@quest_point_accumulated, 0) > @QuestPointPerMilestone) AND @mile_stone_counter < @MaxMilestonePerQuest
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM PlayerMilestone WHERE player_id = @player_id AND quest_id = @quest_id AND milestone_index = @mile_stone_counter)
		BEGIN
			INSERT INTO PlayerMilestone (milestone_index, player_id, quest_id, quest_point, timestamp)
			VALUES (@mile_stone_counter, @player_id, @quest_id, @quest_pt_per_milestone, GETDATE())
		END

		SET @mile_stone_counter += 1
		SET @quest_point_accumulated -= @QuestPointPerMilestone
	END
END