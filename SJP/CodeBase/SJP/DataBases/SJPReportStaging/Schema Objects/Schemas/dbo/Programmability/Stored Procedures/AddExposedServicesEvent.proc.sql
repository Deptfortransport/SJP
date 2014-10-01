CREATE PROCEDURE [dbo].[AddExposedServicesEvent] (@Submitted datetime, @Token varchar(50) = '', @Category varchar(50), @Successful bit, @TimeLogged datetime) AS


INSERT INTO ExposedServicesEvent
                      (Submitted, Token, Category, Successful, TimeLogged)
VALUES     (@Submitted, @Token, @Category, @Successful, @TimeLogged)