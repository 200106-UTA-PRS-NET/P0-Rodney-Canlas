CREATE DATABASE StoreDB;
GO

CREATE SCHEMA PizzaBox;
GO

CREATE TABLE PizzaBox.Account(
	UserID int identity(1,1),
	Username varchar(max) not null,
	Passphrase varchar(max) not null,
	FirstName varchar(max) not null,
	LastName varchar(max) not null,
	constraint pk_Username primary key (UserID)
)
GO

CREATE TABLE PizzaBox.Store(
	StoreID int identity(1,1),
	StoreName varchar(max) not null,
	City varchar(max) not null,
	State varchar(max) not null,
	ZipCode int check(len(ZipCode) = 5),
	constraint pk_StoreID primary key (StoreID)
)
GO

CREATE TABLE PizzaBox.UserOrder(
	OrderID int identity(1,1),
	StoreID int not null,
	UserID int not null,
	--PresetPizza int default(0),
	--CustomPizza int default(0),
	OrderContent varchar(max) not null,
	TotalCost money not null,
	OrderDateTime datetime default(getdate()) not null,
	constraint pk_OrderID primary key (OrderID)
)
GO

insert into PizzaBox.Account
	(Username, Passphrase, FirstName, LastName)
	values
	('admin', 'password', 'Rodney', 'Canlas')

insert into PizzaBox.Store 
	(StoreName, City, State, ZipCode)
	values
	('Giant Caesars', 'Los Angeles', 'California', 90123),
	('Mama Johns', 'Austin', 'Texas', 40123),
	('Pizza Igloo', 'Albany', 'New York', 70233)

select * from PizzaBox.Store;
select * from PizzaBox.Account;
select * from PizzaBox.UserOrder;

