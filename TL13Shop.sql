create database TL13Shop

use TL13Shop

create table Category 
(
	CategoryId int primary key identity(1,1),
	CategoryName varchar(100) not null,
	IsActive bit not null,
	CreatedDate datetime not null
)

create table Brand 
(
	BrandId int primary key identity(1,1),
	BrandName varchar(100) not null,
	BrandImageUrl varchar(max) not null,
	IsActive bit not null,
	CreatedDate datetime not null
)

create table Product 
(
	ProductId int primary key identity(1,1),
	ProductName varchar(100) not null,
	ShortDescription varchar(200) null,
	LongDescription varchar(max) null,
	Price Decimal (20,2) not null,
	Quantity int not null,
	Color varchar(100) not null,
	BrandId int foreign key references Brand(BrandId) on delete cascade not null,
	CategoryId int foreign key references Category(CategoryId) on delete cascade not null,
	Sold int null,
	isActive bit not null,
	CreatedDate datetime not null
)

create table ProductImage
(
	ImageId int primary key identity(1,1),
	ImageUrl varchar(max),
	ProductId int foreign key references Product(ProductId) on delete cascade not null,
	DefaultImage bit null,
)

create table Roles 
(
	RoleId int primary key identity(1,1),
	RoleName varchar(100),
	isActive bit not null
)

create table Users 
(
	UserId int primary key identity(1,1),
	UserName varchar(50) unique not null,
	Password varchar(50) not null,
	PhoneNumber varchar(20) null,
	Email varchar(50) null,
	ImageUrl varchar(max) null,
	RoleId int foreign key references Roles(RoleId) on delete cascade not null,
	CreatedDate datetime not null
)


create table ProductReview 
(
	ReviewId int primary key identity(1,1),
	Rating int not null,
	Comment varchar(max) null,
	ProductId int foreign key references Product(ProductId) on delete cascade not null,
	UserId int foreign key references Users(UserId) on delete cascade not null,
	CreatedDate datetime not null
)

create table WishList
(
	WishListId int primary key identity(1,1),
	UserId int foreign key references Users(UserId) on delete cascade not null,
	ProductId int foreign key references Product(ProductId) on delete cascade not null,
	CreatedDate datetime not null
)

create table Cart
(
	CartId int primary key identity(1,1),
	UserId int foreign key references Users(UserId) on delete cascade not null,
	ProductId int foreign key references Product(ProductId) on delete cascade not null,
	Quantity int not null,
	CreatedDate datetime not null
)

create table Contact 
(
	ContactId int primary key identity(1,1),
	FullName varchar(50) null,
	Email varchar(100) null,
	Message varchar(max) null,
	CreatedDate datetime not null
)

create table OrderStatus 
(
	StatusId int primary key identity(1,1),
	StatusName varchar(50) not null
)

create table Orders
(
	OrderId int primary key identity(1,1),
	UserId int foreign key references Users(UserId) on delete cascade not null,
	ProductId int foreign key references Product(ProductId) on delete cascade not null,
	Quantity int null, 
	Total int null,
	CustomerName varchar(50) not null,
	CustomerPhone varchar(20) not null,
	CustomerAddress varchar(max) not null,
	PaymentMethod varchar(50) not null,
	StatusId int foreign key references OrderStatus(StatusId) on delete cascade not null,
	OrderDate datetime not null,
	IsCancle bit not null default 0
)

Insert into Roles(RoleName, isActive) values 
('admin', 1),
('customer',1)


