create table favorites(
	id 						INT(11)  		NOT NULL AUTO_INCREMENT,
	type 					VARCHAR(255) 	NOT NULL,
	sort 					INT(11)   	    NOT NULL,
	description 			VARCHAR(255),
	creationTime 			TIMESTAMP(0),
  lastModificationTime 	TIMESTAMP(0),

PRIMARY KEY (id) USING BTREE  
);

CREATE TABLE favorites_items (
    id            			INT(11)      	NOT NULL AUTO_INCREMENT,
    fid     	  			INT(11) 		NOT NULL,
    url    					VARCHAR(255) 	NOT NULL,
    target   				VARCHAR(255) 	NOT NULL,
    name   					VARCHAR(255) 	NOT NULL,
    description  			VARCHAR(255),
    sort  					INT(11)      	NOT NULL,
	creationTime 			TIMESTAMP(0) ,
    lastModificationTime 	TIMESTAMP(0),

PRIMARY KEY (id) USING BTREE 
);