# ExercicesWebAPI

### v35
Cr�ation du projet et du mod�le EF (partie Logiciels) sans les relations entre entit�s

### v37
Ajout des relations entre entit�s dans le mod�le EF cr�� pr�c�demment

### v40
Cr�ation d'un jeu de donn�es par le code (appliqu� par migration). Ajout d'un script SQL � ex�cuter sur la base pour compl�ter le jeu de donn�es.

### v55 Requ�tes GET
Cr�ation d'un contr�leur avec actions de lecture pour les logiciels

### v59 Exo de synth�se
Ajout des tables Equipes, Services, Personnes et Metiers dans la baes.  
Ajout d'un petit jeu de donn�es.
Cr�ation d'un contr�leur pour exploiter ces tables.

### v67 Requ�tes POST
Cr�ation d'actions pour l'ajout de releases au format FormData et d'�quipes et personnes au format JSON

### v75 Gestion des erreurs et journalisation
Gestion des erreurs li�es aux contraintes d'int�grit� pour renvoyer des r�ponses 400.   
Journalisation d�taill�e de ces erreurs avec la m�thode d'extension du contr�leur vue dans le cours.

### v77 Validation
Ajout de r�gles de validation impl�ment�es manuellement sur les versions et releases.

### v81 Compl�tion du mod�le
Ajout des tables Activites, ActivitesMetiers, Taches et Travaux.  
Insertion d'un jeu de donn�es.

### v82 Cr�ation du contr�leur Taches
Cr�ation du contr�leur avec actions de lecture et d'ajout pour les t�ches et travaux.

### v83 Ajout de r�gles m�tier
Ajout de r�gles m�tier pour la cr�ation de t�ches et travaux.

### v94 Requ�tes DELETE, PUT et PATCH
Ajout d'actions DELETE et PUT pour la suppression et la modification de t�ches, travaux et personnes.  
Ajout d'une action PATCH pour la modification de versions.

### v100 Authentification
Cr�ation d'un serveur d'identit� avec Duende IdentityServer  
S�curisation de l'API et cr�ation d'une appli cliente de test Blazor WASM avec Backend For Frontend

### v114 Autorisation
Ajout d'utilisateurs et de revendications sur le serveur d'identit�.  
Configuration de l'autorisation sur l'API pour la gestion des t�ches et des �quipes.
