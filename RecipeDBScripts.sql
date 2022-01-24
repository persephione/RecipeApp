CREATE DATABASE RecipeDB

GO

USE RecipeDB;
GO

-- create table
CREATE TABLE Recipes
(
	RecipeId int IDENTITY(1,1) NOT NULL,
	RecipeName nvarchar(255) NOT NULL,
	Ingredients nvarchar(1000) NOT NULL,
	Instructions nvarchar(4000) NOT NULL,
	CONSTRAINT PK_RecipeID PRIMARY KEY(RecipeID)
)

-- insert values into tables
INSERT INTO Recipes (RecipeName, Ingredients, Instructions) VALUES
('Brownies', '12 oz Chocolate Chips, 1 cup Flour', 'Brownie Instructions'),
('Banana Bread', '3 Bananas, 1 tsp Vanilla Extract', 'BB Instructions'),
('Chocolate Cheesecake', '23 oz Cream Cheese, 4 Eggs', 'Cheesecake Instructions'),
('Oatmeal Raisin Cookies', '1 cup Raisins, 1 1/2 cup Oats', 'ORC Instructions'),
('Oreo Balls', '32 Oreos, 12 oz Melting Chocolate Wafers', 'Oreo Ball Instructions')
GO

-- create sprocs
CREATE PROCEDURE sp_GetRecipes AS SELECT RecipeID, RecipeName FROM Recipes
GO

CREATE PROCEDURE sp_GetRecipeDetailById @RecipeId int
AS
SELECT RecipeName, Ingredients, Instructions 
FROM Recipes
WHERE RecipeId = @RecipeId
GO

CREATE PROCEDURE sp_InsertRecipe
  @RecipeName varchar(100),
  @Ingredients varchar(500),
  @Instructions varchar(8000),
  @RecipeID int OUT
AS
BEGIN
INSERT INTO Recipes VALUES(@RecipeName, @Ingredients, @Instructions)
SET @RecipeID = SCOPE_IDENTITY()
RETURN @RecipeID
END
GO

CREATE PROCEDURE sp_UpdateRecipe
	@RecipeID int,
	@RecipeName varchar(100),
	@Ingredients varchar(500),
	@Instructions varchar(8000)
AS
BEGIN
UPDATE RECIPES
    SET RecipeName = @RecipeName,
		Ingredients = @Ingredients,
		Instructions = @Instructions
    WHERE RecipeID = @RecipeID
END
GO

CREATE PROCEDURE sp_DeleteRecipe
	@RecipeID int
AS
BEGIN
	DELETE FROM RECIPES
	WHERE RecipeID = @RecipeID
END
GO


