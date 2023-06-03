ALTER TRIGGER ActualizarBloqueTrigger
ON competencia_bloque_clasifica
AFTER UPDATE
AS
BEGIN
			print ('Entra')

    -- Declarar una variable para almacenar los registros actualizados
    DECLARE @IdCompetencia INT; -- Cambiar el tipo de datos según el tipo de la columna id_competencia
	DECLARE @id_compe_bloque_ingresado INT;
    -- Obtener el id_competencia del registro actualizado
    SELECT @IdCompetencia = id_com FROM inserted;
	SELECT @id_compe_bloque_ingresado = id_compe_bloque_cla FROM inserted;


    -- Guardar los registros con el mismo id_competencia en la variable
    DECLARE @RegistrosTabla TABLE (
        -- Definir las columnas necesarias para almacenar los datos de la tabla
        -- Cambiar las columnas según la estructura de la tabla competencia_bloque_clasifica
        id_compe_bloque_cla int,
        zona_cla int,
		zona_intentos_cla int,
		top_cla int,
		top_intentos_cla int,
		puesto int
        
    );

    INSERT INTO @RegistrosTabla (id_compe_bloque_cla, zona_cla,zona_intentos_cla,top_cla,top_intentos_cla,puesto )
    SELECT id_compe_bloque_cla, zona_cla,zona_intentos_cla,top_cla,top_intentos_cla,puesto 
    FROM competencia_bloque_clasifica
    WHERE id_com = @IdCompetencia;

    -- Realizar cualquier otra operación que desees con los registros guardados en la variable

	IF NOT EXISTS (SELECT 1 FROM @RegistrosTabla WHERE puesto IS NOT NULL)
    BEGIN
        -- Asignar el puesto 1 al registro agregado
        UPDATE competencia_bloque_clasifica
        SET puesto = 1, clasi_bloque = 1
        WHERE id_com = @IdCompetencia;
        RETURN; -- Salir del trigger si se asignó el puesto 1
    END;


	-- Declaracion de datos para comparar
	DECLARE @zona_cla_ingresado INT;
	DECLARE @zona_intentos_ingresado INT;
	DECLARE @top_cla_ingresado INT;
	DECLARE @top_intentos_ingresado INT;
	DECLARE @zona_cla_tabla INT;
	DECLARE @zona_intentos_tabla INT;
	DECLARE @top_cla_tabla INT
	DECLARE @top_intentos_tabla INT;
	DECLARE @id_comple_bloque_tabla INT;
	DECLARE @puesto_bloque INT;


	SELECT @zona_cla_ingresado = zona_cla FROM inserted;
	SELECT @zona_intentos_ingresado = zona_intentos_cla FROM inserted;
	SELECT @top_cla_ingresado = top_cla FROM inserted;
	SELECT @top_intentos_ingresado = top_intentos_cla FROM inserted;

	DECLARE @aux BIT;
	DECLARE @comprobracion BIT;

	-- Declaración de variables para el bucle
	DECLARE @Counter INT = 1;
	DECLARE @TotalRecords INT = (SELECT COUNT(*) FROM @RegistrosTabla);

	-- Bucle WHILE para recorrer los registros
	WHILE @Counter <= @TotalRecords
	BEGIN
		SELECT 
			@zona_cla_tabla = zona_cla,
			@zona_intentos_tabla = zona_intentos_cla,
			@top_cla_tabla = top_cla,
			@top_intentos_tabla = top_intentos_cla,
			@puesto_bloque = puesto
		FROM @RegistrosTabla
		WHERE @@ROWCOUNT = @Counter;
		IF (@comprobracion = 1)
		BEGIN
		IF (@zona_cla_ingresado = @zona_cla_tabla AND @zona_intentos_ingresado = @zona_intentos_tabla AND @top_cla_ingresado = @top_cla_tabla AND @top_intentos_ingresado = @top_intentos_tabla)
		BEGIN
			print ('Entra')
			EXEC dbo.ActualizarPuestos @id_compe_bloque_ingresado, @id_comple_bloque_tabla;
			SET @comprobracion = 0;
			SET @aux = 0;
		END
		ELSE
		BEGIN
			IF (@zona_cla_ingresado > @zona_cla_tabla)
			BEGIN
				EXEC dbo.ActualizarPuestos @id_compe_bloque_ingresado, @id_comple_bloque_tabla;
				SET @comprobracion = 0;
				SET @aux = 0;
			END
			ELSE
			BEGIN
				IF (@zona_cla_ingresado = @zona_cla_tabla)
				BEGIN
					IF (@top_cla_ingresado > @top_cla_tabla)
					BEGIN
						EXEC dbo.ActualizarPuestos @id_compe_bloque_ingresado, @id_comple_bloque_tabla;
						SET @comprobracion = 0;
						SET @aux = 0;
					END
					ELSE
					BEGIN
						IF (@top_cla_ingresado = @top_cla_tabla)
						BEGIN
							IF (@zona_intentos_ingresado < @zona_intentos_tabla)
							BEGIN
								EXEC dbo.ActualizarPuestos @id_compe_bloque_ingresado, @id_comple_bloque_tabla;
								SET @comprobracion = 0;
								SET @aux = 0;
							END
							ELSE
							BEGIN
								IF (@zona_intentos_ingresado = @zona_intentos_tabla)
								BEGIN
									IF (@top_intentos_ingresado < @top_intentos_tabla)
									BEGIN
										EXEC dbo.ActualizarPuestos @id_compe_bloque_ingresado, @id_comple_bloque_tabla;
										SET @comprobracion = 0;
										SET @aux = 0;
									END
								END
							END
						END
					END
				END
			END
		END

			
		END;
		ELSE
		BEGIN
			
			UPDATE competencia_bloque_clasifica
			SET puesto = @puesto_bloque +1,
				clasi_bloque = CASE WHEN @puesto_bloque > 6 THEN 0 ELSE 1 END
			WHERE id_compe_bloque_cla = @id_comple_bloque_tabla AND puesto IS NOT NULL;

		END;

		

		-- Realizar las operaciones o comparaciones necesarias con los valores obtenidos

		-- Incrementar el contador del bucle
		SET @Counter = @Counter + 1;
	END;

	IF @aux = 1
	BEGIN
		UPDATE competencia_bloque_clasifica
		SET puesto = (SELECT COUNT(*) FROM competencia_bloque_clasifica),
			clasi_bloque = CASE WHEN (SELECT COUNT(*) FROM competencia_bloque_clasifica) <= 6 THEN 1 ELSE 0 END
		WHERE id_compe_bloque_cla = @id_compe_bloque_ingresado;
	END



END;
