-- =============================================
-- Author:	 Emmanuel J Sanchez	
-- Create date: 2/23/2016
-- Description: Used to calculate how many days in the given period are workays, and thus
--			 , payable, acording to how many work days per week there are.
-- =============================================
CREATE FUNCTION PayableDaysInPeriod
(
	-- Add the parameters for the function here
	@WorkDaysPerWeek int,
	@PeriodStartDate DATE,
	@PeriodEndDate DATE
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @PayableDays INT;

	-- Add the T-SQL statements to compute the return value here
	SET @PayableDays = CASE	  
					   WHEN @WorkDaysPerWeek = 5 THEN 
							  (DATEDIFF(dd, @PeriodStartDate, @PeriodEndDate) + 1)
							  -(DATEDIFF(wk, @PeriodStartDate, @PeriodEndDate) * 2)
							  -(CASE WHEN DATENAME(dw, @PeriodStartDate) = 'Sunday' THEN 1 ELSE 0 END)
							  -(CASE WHEN DATENAME(dw, @PeriodEndDate) = 'Saturday' THEN 1 ELSE 0 END)
					   WHEN @WorkDaysPerWeek = 6 THEN
							 (DATEDIFF(dd, @PeriodStartDate, @PeriodEndDate) + 1)
							 -(DATEDIFF(wk, @PeriodStartDate, @PeriodEndDate))
							 -(CASE WHEN DATENAME(dw, @PeriodStartDate) = 'Sunday' THEN 1 ELSE 0 END)
					   ELSE
						  0
				    END
							

	-- Return the result of the function
	RETURN @PayableDays

END