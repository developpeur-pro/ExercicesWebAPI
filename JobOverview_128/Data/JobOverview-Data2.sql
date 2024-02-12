print 'Table Services'
insert Services (Code, Nom) values
('DEV', 'Développement'),
('TEST', 'Test'),
('SC', 'Support Client'),
('MKT', 'Marketing')
go

print 'Table Equipes'
insert Equipes (Code, Nom, CodeFiliere, CodeService) values
('BIOH_MKT', 'Marketing Biologie humaine', 'BIOH', 'MKT'),
('BIOH_DEV', 'Dev Biologie humaine', 'BIOH', 'DEV'),
('BIOH_TEST', 'Test Biologie humaine', 'BIOH', 'TEST'),
('BIOH_SL', 'Support Biologie humaine', 'BIOH', 'SC')
go

print 'Table Metiers'
insert Metiers (Code, CodeService, Titre) values
('ANA', 'MKT', 'Analyste'),
('CDP', 'DEV', 'Chef de Projet'),
('DEV', 'DEV', 'Développeur'),
('DES', 'MKT', 'Designer'),
('TES', 'TEST', 'Testeur')
go

print 'Table Personnes'
insert Personnes (Pseudo, Prenom, Nom, CodeMetier, CodeEquipe, Manager, TauxProductivite) values
('JROUSSET', 'Joseph', 'ROUSSET', 'ANA', 'BIOH_MKT', null, 1),
('GLECLERCQ', 'Gaëlle', 'LECLERCQ', 'ANA', 'BIOH_MKT', 'JROUSSET', 0.8),
('AFERRAND', 'Aurélie', 'FERRAND', 'ANA', 'BIOH_MKT', 'JROUSSET', 1),
('MWEBER', 'Marion', 'WEBER', 'DES', 'BIOH_MKT', 'JROUSSET', 1),
('BNORMAND', 'Bertrand', 'NORMAND', 'CDP', 'BIOH_DEV', null, 1),
('RFORTIER', 'Rémy', 'FORTIER', 'DEV', 'BIOH_DEV', 'BNORMAND', 1),
('LBREGON', 'Léo', 'BREGON', 'DEV', 'BIOH_DEV', 'BNORMAND', 0.5),
('RBEAUMONT', 'Romane', 'BEAUMONT', 'DEV', 'BIOH_DEV', 'BNORMAND', 1),
('APARENT', 'Agnès', 'PARENT', 'TES', 'BIOH_TEST', null, 1),
('HKLEIN', 'Hugo', 'KLEIN', 'TES', 'BIOH_TEST', 'APARENT', 1),
('NPROVIN', 'Nathan', 'PROVIN', 'TES', 'BIOH_TEST', 'APARENT', 1)
go
