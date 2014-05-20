
CREATE TABLE files (
	filename TEXT PRIMARY KEY,
	type TEXT NOT NULL
);

CREATE TABLE configs (
	filename TEXT,
	configname TEXT,
	
	UOM TEXT,
	ID TEXT,
	Description TEXT,
	DesignedBy TEXT,
	DrawDate TEXT,
	Type TEXT,
	AltQty REAL,
	EngApproval TEXT,
	EngApprDate TEXT,
	MfgApproval TEXT,
	MfgApprDate TEXT,
	QAApproval TEXT,
	QAApprDate TEXT,
	PurchApproval TEXT,
	PurchApprDate TEXT,
	Material TEXT,
	Finish TEXT,
	Coating TEXT,
	Notes TEXT,
	Revision TEXT,
	Ecos TEXT,
	EcoRevs TEXT,
	Zone TEXT,
	EcoDescriptions TEXT,
	EcoDates TEXT,
	EcoChks TEXT,
	P_M TEXT,
	PlaceHoldFlag INTEGER,
	
	PRIMARY KEY (filename,configname),
	
	FOREIGN KEY (filename) REFERENCES files(filename)
);

CREATE TABLE adjacency (
	pname TEXT,
	pconfig TEXT,
	cname TEXT,
	cconfig TEXT,
	count INTEGER,
	
	PRIMARY KEY (pname,pconfig,cname,cconfig),
	
	FOREIGN KEY (pname) REFERENCES configs(filename),
	FOREIGN KEY (pconfig) REFERENCES configs(configname),
	FOREIGN KEY (cname) REFERENCES configs(filename),
	FOREIGN KEY (cconfig) REFERENCES configs(configname)
);


INSERT INTO files VALUES('C:\pwa\Designed\Common\Documentation-Graphics\Documents\X000636.Universal Delivery Training.SLDPRT','SLDPRT');
INSERT INTO files VALUES('C:\pwa\Designed\AZ300\Documentation-Graphics\Documents\EM0004.Owners Manual Packet AZ300.SLDASM','SLDASM');

insert into configs values ('C:\pwa\Designed\Common\Documentation-Graphics\Documents\X000636.Universal Delivery Training.SLDPRT','Default','EA','X000636','Universal Delivery and Training Check-off Sheet','JKN','2007-02-06','P',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'Designed by PJ Mead','0',NULL,NULL,NULL,NULL,NULL,NULL,'M',NULL);
insert into configs values ('C:\pwa\Designed\AZ300\Documentation-Graphics\Documents\EM0004.Owners Manual Packet AZ300.SLDASM','Default','EA','EM0004','Owners Manual Packet','MHT','2004-06-24','K',NULL,'JTH','2004-06-24','MLG','2004-06-25','CLW','2004-06-28','DRF','2004-06-30',NULL,NULL,NULL,NULL,'-4','none none 400 10121','-1 -2 -3 -4',NULL,'Modified part descriptions; added EM0006 Added M10F5 and M10F6 Added M10G1 for JD engine Replace M10F5 and M10F6 w/ X000636, EM0006 w/ X000030','02/15/05 07/20/05 08/18/05 03/05/07','MHT CRB MHT MHT','M',NULL);

INSERT INTO adjacency VALUES('C:\pwa\Designed\AZ300\Documentation-Graphics\Documents\EM0004.Owners Manual Packet AZ300.SLDASM','Default','C:\pwa\Designed\Common\Documentation-Graphics\Documents\X000636.Universal Delivery Training.SLDPRT','Default',1);








