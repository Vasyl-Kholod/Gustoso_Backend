use Gustoso;
create table Rating(
	id int not null primary key identity(0,1),
	userName nvarchar(500) not null,
	slideName nvarchar(20) not null,
	ratingValue int default(1) 
);