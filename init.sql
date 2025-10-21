CREATE TABLE Users (
        id SERIAL PRIMARY KEY,
        email VARCHAR(255) UNIQUE NOT NULL,
        password TEXT NOT NULL,
        username VARCHAR(100) NOT NULL,
        preference int NOT NULL
);

CREATE TABLE Tasks (
        id SERIAL PRIMARY KEY,
        description TEXT,
        completed BOOLEAN,
        task_date DATETIME,
        owner_id INT REFERENCES Users(id) ON DELETE SET NULL -- i dont want to delete the task even user is gone --  
);

CREATE VIEW UserTask AS
        SELECT U.username, T.description, T.completed, T.task_date
        FROM Users U
        JOIN Tasks T ON T.id = U.id
