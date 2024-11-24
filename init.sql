-- Check if the user 'root'@'%' exists
SET @user_exists = (SELECT COUNT(*) FROM mysql.user WHERE user = 'root' AND host = '%');

-- If the user exists, update the password; otherwise, create the user
IF @user_exists > 0 THEN
    ALTER USER 'root'@'%' IDENTIFIED BY 'passord123';
ELSE
    CREATE USER 'root'@'%' IDENTIFIED BY 'passord123';
END IF;

-- Grant necessary privileges to the user
GRANT ALL PRIVILEGES ON 'kartverket_tables'.* TO 'root'@'%';
FLUSH PRIVILEGES;
