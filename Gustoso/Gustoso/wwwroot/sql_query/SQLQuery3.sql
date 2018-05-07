use Gustoso;
create table Reservations(
	id int not null primary key identity(0,1),
	clientName nvarchar(100) not null,
	clientPhone nvarchar(30) not null,
	clientEmail nvarchar(500) not null,
	tableNumber int not null,
	dateOfReservation nvarchar(100) not null,
	isConfirmed nvarchar not null default('false')
);