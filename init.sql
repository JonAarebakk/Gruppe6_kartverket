﻿-- Check if the user 'root'@'%' exists
SELECT EXISTS(SELECT 1 FROM mysql.user WHERE user = 'root' AND host = '%') INTO @user_exists;

-- If the user exists, update the password; otherwise, create the user
IF @user_exists THEN
    ALTER USER 'root'@'%' IDENTIFIED BY 'passord123';
ELSE
    CREATE USER 'root'@'%' IDENTIFIED BY 'passord123';
END IF;

-- Grant necessary privileges to the user
GRANT ALL PRIVILEGES ON kartverket_tables.* TO 'root'@'%';
FLUSH PRIVILEGES;
