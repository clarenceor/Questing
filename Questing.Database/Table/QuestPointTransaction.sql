SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[QuestPointTransaction](
	[tran_id] [int] IDENTITY(1,1) NOT NULL,
	[quest_id] [int] NOT NULL,
	[player_id] [varchar](30) NOT NULL,
	[quest_point_earned] [decimal](18, 2) NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_QuestPointTransaction] PRIMARY KEY CLUSTERED 
(
	[tran_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuestPointTransaction]  WITH CHECK ADD  CONSTRAINT [FK_QuestPointTransaction_quest] FOREIGN KEY([quest_id])
REFERENCES [dbo].[Quest] ([quest_id])
GO

ALTER TABLE [dbo].[QuestPointTransaction] CHECK CONSTRAINT [FK_QuestPointTransaction_quest]
GO