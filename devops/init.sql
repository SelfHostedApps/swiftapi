CREATE TABLE Users (
        id SERIAL PRIMARY KEY,
        email VARCHAR(255) UNIQUE NOT NULL,
        password TEXT NOT NULL,
        username VARCHAR(100) NOT NULL,
        preference int NOT NULL,
        roles VARCHAR(100) default 'user'
);

CREATE TABLE Tasks (
        id SERIAL PRIMARY KEY,
        description TEXT,
        completed BOOLEAN DEFAULT FALSE,
        task_date TIMESTAMP,
        owner_id INT REFERENCES Users(id) ON DELETE SET NULL -- i dont want to delete the task even user is gone --  
);

CREATE OR REPLACE VIEW UserTask AS
SELECT 
    U.username,
    T.description,
    T.completed,
    T.task_date
FROM Tasks T
LEFT JOIN Users U ON T.owner_id = U.id;
