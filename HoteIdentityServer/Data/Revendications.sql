-- Mise à jour des utilisateurs
update AspNetUsers set Id = 'LBREGON', UserName = 'Laura Brégon'
where email = 'lbregon@joboverview.fr'

update AspNetUsers set Id = 'PROI', UserName = 'Patrick Roi'
where email = 'proi@joboverview.fr'

-- Création de revendications
insert AspNetUserClaims(UserId, ClaimType, ClaimValue) values
('BNORMAND', 'métier', 'CDP'),
('BNORMAND', 'manager', ''),
('LBREGON', 'métier', 'DEV'),
('PROI', 'métier', 'CDS'),
('PROI', 'manager', '')

select * from AspNetUsers
select * from AspNetUserClaims