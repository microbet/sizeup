// Found in szupit-stg-db01.gisplanning.net:/Users/acooper/Documents .
// Created 4/20/2016, last modified 5/3/2016.
// The format of input file ("DEUTSCHE_OUTPUT.txt") might be that of
// https://s3-us-west-2.amazonaws.com/sizeup-datasources/cerved/DEUTSCHE_0_NO_OPERATIVE.txt

try {
    var lineCount = 0,
        request = {},
        readCount = 0;
    sql = require('mssql'),
        connection = null,
        fs = require('fs'),
        LineByLineReader = require('line-by-line'),

        insertLine = function (line, callback) {
            if (lineCount < 1) {
                lineCount++;
                return;
            }
            var lineObj = line.trim(' ').split('|');
            callback(lineObj, insertBusiness);
        },
        insertIndustry = function (lineObj, callback) {
            new sql.Request().query(`IF COALESCE(NULLIF('${lineObj[18]}',''),NULLIF('${lineObj[38]}','')) IS NOT NULL AND NOT EXISTS (SELECT [CervedAteco],[OfficialAteco] FROM [SizeUpDataIT].[dbo].[Industry1]
                WHERE (CervedAteco = '${lineObj[18]}' AND OfficialAteco = '${lineObj[38]}') OR (Name = '${lineObj[19].replace(/'/g, "''")}'))
                BEGIN
                  INSERT INTO [SizeUpDataIT].[dbo].[Industry1] (Name, CervedAteco, OfficialAteco, isActive, SEOKey)
                  VALUES ( '${lineObj[19].replace(/'/g, "''")}',
                        '${lineObj[18].replace(/'/g, "''")}',
                        '${lineObj[38].replace(/'/g, "''")}',
                        1,
                        '${lineObj[19].toLowerCase().replace(/'/g, "''").replace(/ /g, '_')}' )
                END`
                , function (err) {
                    if (err !== undefined)console.log("Error Inserting Industry Occurred at line" + lineCount + " " + err);
                    callback(lineObj, insertBusinessData);
                });
        },
        insertBusiness = function (lineObj, callback) {
            new sql.Request().query(`INSERT INTO [SizeUpDataIT].[dbo].[Business1]
                ([Name]
                    ,[Address]
                    ,[City]
                    ,[Phone]
                    ,[CervedId]
                    ,[YearAppeared]
                    ,[Lat]
                    ,[Long]
                    ,[IndustryId]
                    ,[ZipCode]
                    ,[StateId]
                    ,[CountyId]
                    ,[SEOKey]
                    ,[IsActive])
                VALUES (
                    '${lineObj[6].replace(/'/g, "''")}',
                    '${lineObj[9].replace(/'/g, "''")}',
                    '${lineObj[11].replace(/'/g, "''")}',
                    '${lineObj[23].replace(/'/g, "''")}',
                    '${lineObj[0].replace(/'/g, "''")}',
                    ${!lineObj[22].trim() ? null : lineObj[22].substring(0, 4)},
                    ${!lineObj[15].trim() ? null : lineObj[15].replace(/,/g, '.').substring(0, (lineObj[15].indexOf(',')) + 7)},
                    ${!lineObj[16].trim() ? null : lineObj[16].replace(/,/g, '.').substring(0, (lineObj[16].indexOf(',')) + 7)},
                   (SELECT top 1 ID from Industry1 where OfficialAteco = '${lineObj[38]}' OR CervedAteco = '${lineObj[18]}'),
                    '${lineObj[10].replace(/'/g, "''")}',
                    (SELECT top 1 ID from State where Name = '${lineObj[14] === '' ? null : lineObj[14].replace(/'/g,
                "''")}'),
                    (SELECT top 1 ID from County where abbreviation = '${lineObj[1]}'),
                    '${lineObj[6].toLowerCase().replace(/'/g, "''").replace(/ /g, '_')}',
                    ${lineObj[36].trim().toLowerCase() === 'a' ? 1 : 0 })`
                , function (err) {
                    if (err !== undefined)console.log("Error Occurred importing business at line" + lineCount + " " + err);
                    callback(lineObj);
                });
        },
        insertBusinessData = function (lineObj) {
            new sql.Request().query(`INSERT INTO [SizeUpDataIT].[dbo].[BusinessData1]
           ([Year]
           ,[Quarter]
           ,[IndustryId]
           ,[GeographicLocationId]
           ,[BusinessId]
           ,[BranchTypeCode]
           ,[Revenue]
           ,[Employees]
           ,[YearStarted]
           ,[OperatingCost]
           ,[NetProfit]
           ,[DebtEquityRatio]
           ,[NetWorth]
           ,[CostOfPersonnel]
           ,[TotalBranchesNumber])
     VALUES
           (2016
           ,1
           ,(SELECT top 1 ID from Industry1 where OfficialAteco = '${lineObj[38]}' OR CervedAteco = '${lineObj[18]}')
           ,(SELECT top 1 ID from GeographicLocation WHERE Shortname = '${lineObj[10]}' )
           ,(SELECT top 1 ID FROM business1 WHERE Name = '${lineObj[6].replace(/'/g, "''")}' AND [Address] = '${lineObj[9].replace(/'/g, "''")}')
           ,${!lineObj[3].trim() ? null : lineObj[3]}
           ,${!lineObj[20].trim() ? ( !lineObj[25].trim() ? null : lineObj[25] ) : lineObj[20]}
           ,${!lineObj[21].trim() ? null : lineObj[21]}
           ,${!lineObj[22].trim() ? null : lineObj[22].substring(0, 4)}
           ,${!lineObj[27].trim() ? null : lineObj[27]}
           ,${!lineObj[28].trim() ? null : lineObj[28]}
           ,${!lineObj[27].trim() ? null : lineObj[27].trim().replace(/,/g, '.')}
           ,${!lineObj[31].trim() ? null : lineObj[31]}
           ,${!lineObj[26].trim() ? null : lineObj[26]}
           ,${!lineObj[37].trim() ? null : lineObj[37]}
           )`
                , function (err) {
                    if (err !== undefined)console.log("Error Occurred Importing businessdata at line" + lineObj[10] + " " + lineCount + " " + err);
                    lineCount++;
                });
        },
        insertCervedAteco = function(lineObj) {
            new sql.Request().query(`INSERT INTO [RawDataIT].[dbo].[DeutscheBankCerved]
            ([ID_CERVEDGROUP]
                ,[CCIAA]
                ,[NREA]
                ,[TIPO_SEDE]
                ,[CODICE_FISCALE]
                ,[PARTITA_IVA]
                ,[DENOMINAZIONE]
                ,[DESCRIZIONE_NATURA_GIURIDICA]
                ,[TIPOLOGIA_TIPOLOGIA_SEDE]
                ,[INDIRIZZO]
                ,[CAP]
                ,[COMUNE]
                ,[FRAZIONE]
                ,[PROVINCIA]
                ,[REGIONE]
                ,[LATITUDINE]
                ,[LONGITUDINE]
                ,[PRECISIONE]
                ,[CODICE_ATECO_PRIMARIO]
                ,[DESCRIZIONE_ATECO_PRIMARIO]
                ,[FATTURATO_UL]
                ,[ADDETTI]
                ,[DATA_ISCRIZIONE_CCIAA]
                ,[TELEFONO_I]
                ,[DATA_CHIUSURA]
                ,[FATTURATO]
                ,[COSTO_DEL_PERSONALE]
                ,[COSTI_OPERATIVI]
                ,[UTILE]
                ,[DEBITI_SU_PATRIMONIO_NETTO]
                ,[CII1_CLIENTI]
                ,[D7_DEBITI_VS_FORNITORI]
                ,[C_CIRCOLANTE]
                ,[DEBITI_A_BREVE]
                ,[PATRIMONIO_NETTO]
                ,[FLAG_MON]
                ,[STATO_ATTIVITA]
                ,[NUMERO_UL]
                ,[ATECO_ISTAT])
            VALUES (
                    '${formatSql(lineObj[0])}',
                    '${formatSql(lineObj[1])}',
                    '${formatSql(lineObj[2])}',
                    '${formatSql(lineObj[3])}',
                    '${formatSql(lineObj[4])}',
                    '${formatSql(lineObj[5])}',
                    '${formatSql(lineObj[6])}',
                    '${formatSql(lineObj[7])}',
                    '${formatSql(lineObj[8])}',
                    '${formatSql(lineObj[9])}',
                    '${formatSql(lineObj[10])}',
                    '${formatSql(lineObj[11])}',
                    '${formatSql(lineObj[12])}',
                    '${formatSql(lineObj[13])}',
                    '${formatSql(lineObj[14])}',
                    '${formatSql(lineObj[15])}',
                    '${formatSql(lineObj[16])}',
                    '${formatSql(lineObj[17])}',
                    '${formatSql(lineObj[18])}',
                    '${formatSql(lineObj[19])}',
                    '${formatSql(lineObj[20])}',
                    '${formatSql(lineObj[21])}',
                    '${formatSql(lineObj[22])}',
                    '${formatSql(lineObj[23])}',
                    '${formatSql(lineObj[24])}',
                    '${formatSql(lineObj[25])}',
                    '${formatSql(lineObj[26])}',
                    '${formatSql(lineObj[27])}',
                    '${formatSql(lineObj[28])}',
                    '${formatSql(lineObj[29])}',
                    '${formatSql(lineObj[30])}',
                    '${formatSql(lineObj[31])}',
                    '${formatSql(lineObj[32])}',
                    '${formatSql(lineObj[33])}',
                    '${formatSql(lineObj[34])}',
                    '${formatSql(lineObj[35])}',
                    '${formatSql(lineObj[36])}',
                    '${formatSql(lineObj[37])}',
                    '${formatSql(lineObj[38])}')
					`

                , function (err) {
                    if (err !== undefined)console.log("Error Occurred importing business at line" + lineCount + " " + err);

                });
        },
        createReader = function () {
            var rl = new LineByLineReader('DEUTSCHE_OUTPUT.txt', {skipEmptyLines: true});

            rl.on('line', (line) => {

                    rl.pause();
                    setTimeout(function () {
                        //insertLine(line, insertIndustry);
                        insertLine(line, insertCervedAteco);
                        rl.resume();
                    }, 1);
                })
                .on('error', (err) => {
                    throw Error(err);
                });
            return rl;
        },
        formatSql = function(sql){
        return sql == null ? null : sql.replace(/'/g, "''")
    };


    sql.connect("mssql://sizeup:sizeupmaps4u!@szupit-stg-db01.gisplanning.net/SizeUpDataIT")
        .then(function (conn) {
            var connection = conn;
            var rl = createReader();

        });

}
catch (err) {
    console.log(`Import failed on line [${lineCount}] ${err}`);
}
;
