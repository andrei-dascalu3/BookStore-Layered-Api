USE BookStoreTest;
GO

IF OBJECT_ID('dbo.Books', 'U') IS NOT NULL
BEGIN
    DELETE FROM dbo.Books;

    INSERT INTO dbo.Books (Title, AuthorName, Genre, Price, PublishedDate)
    VALUES 
    ('1984', 'George Orwell', 'Dystopian', 9.99, '1949-06-08'),
    ('Animal Farm', 'George Orwell', 'Satire', 7.99, '1945-08-17'),
    ('Pride and Prejudice', 'Jane Austen', 'Romance', 8.49, '1813-01-28'),
    ('Adventures of Huckleberry Finn', 'Mark Twain', 'Adventure', 6.99, '1884-12-10');
END;
GO