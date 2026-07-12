USE BookStoreTest;
GO

-- Create the Books table if it doesn't exist
IF OBJECT_ID('dbo.Books', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Books (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(250) NOT NULL,
        AuthorName NVARCHAR(250) NOT NULL,
        Genre NVARCHAR(100) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        PublishedDate DATE NOT NULL
    );
END;
GO