CREATE TABLE IF NOT EXISTS prices(
	id INT PRIMARY KEY,
	price REAL NOT NULL,
	collect_date TEXT NOT NULL,
	currency TEXT CHECK(currency IN ("DOLLARS")) NOT NULL
);