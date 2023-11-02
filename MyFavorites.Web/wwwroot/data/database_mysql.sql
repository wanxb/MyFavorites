create table favorites(
	id 						INT(11)  		NOT NULL AUTO_INCREMENT,
	type 					VARCHAR(255) 	NOT NULL,
	sort 					INT(11) NOT  	NOT NULL,
	description 			VARCHAR(255) 	NULL,
	creationTime 			TIMESTAMP(0) 	NOT NULL,
    lastModificationTime 	TIMESTAMP(0) 	NOT NULL,

PRIMARY KEY (id) USING BTREE  
);

CREATE TABLE favorites_items (
    id            			INT(11)      	NOT NULL AUTO_INCREMENT,
    f_id     	  			INT(11) 		NOT NULL,
    url    					VARCHAR(255) 	NOT NULL,
    target   				VARCHAR(255) 	NOT NULL,
    name   					VARCHAR(255) 	NOT NULL,
    description  			VARCHAR(255)    NULL,
    sort  					INT(11)      	NOT NULL,
	creationTime 			TIMESTAMP(0) 	NOT NULL,
    lastModificationTime 	TIMESTAMP(0) 	NOT NULL,

PRIMARY KEY (id) USING BTREE 
);