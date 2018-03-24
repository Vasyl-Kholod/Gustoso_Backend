use Gustoso;
create table ContactUs (
	id int not null primary key identity(0,1),
	clientName nvarchar(100) not null,
	clientEmail nvarchar(500) not null,
	clientSubject nvarchar(500) not null,
	clientMessage nvarchar(2000) not null
);