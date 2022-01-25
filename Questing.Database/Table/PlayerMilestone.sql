SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PlayerMilestone](
	[player_id] [varchar](30) NOT NULL,
	[quest_id] [int] NOT NULL,
	[milestone_index] [int] NOT NULL,
	[quest_point] [decimal](18, 2) NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_PlayerMilestone] PRIMARY KEY CLUSTERED 
(
	[player_id] ASC,
	[quest_id] ASC,
	[milestone_index] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PlayerMilestone]  WITH CHECK ADD  CONSTRAINT [FK__PlayerMil__quest__5812160E] FOREIGN KEY([quest_id])
REFERENCES [dbo].[Quest] ([quest_id])
GO

ALTER TABLE [dbo].[PlayerMilestone] CHECK CONSTRAINT [FK__PlayerMil__quest__5812160E]
GO