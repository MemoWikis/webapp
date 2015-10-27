#Beispiel von: http://www.theanalysisfactor.com/r-tutorial-5/
A <- structure(list(Time = c(0, 1, 2, 4, 6, 8, 9, 10, 11, 12, 13, 14, 15, 
                             16, 18, 19, 20, 21, 22, 24, 25, 26, 27, 28, 29, 30), 
Counts = c(126.6, 101.8, 71.6, 101.6, 68.1, 62.9, 45.5, 41.9, 46.3, 34.1, 38.2, 41.7, 24.7, 41.5, 36.6, 
           19.6, 22.8, 29.6, 23.5, 15.3, 13.4, 26.8, 9.8, 18.8, 25.9, 19.3)), 
.Names = c("Time", "Counts"), 
row.names = c(1L, 2L, 3L, 5L, 7L, 9L, 10L, 11L, 12L, 13L, 14L, 15L, 
              16L, 17L, 19L, 20L, 21L, 22L, 23L, 25L, 26L, 27L, 28L, 29L, 30L,31L), 
class = "data.frame")

attach(A)
names(A)
exponential.model <- lm(log(Counts)~ Time)
summary(exponential.model)
timevalues <- seq(0, 30, 0.1)
Counts.exponential2 <- exp(predict(exponential.model,list(Time=timevalues)))
plot(Time, Counts,pch=16)
lines(timevalues, Counts.exponential2,lwd=2, col = "red", xlab = "Time (s)", ylab = "Counts")

exp(4.55525)
plot(function(x){95.13053*(exp(1))^(-0.06392*x)}, 0,30, col="green", add=TRUE)

#Access values (http://data.princeton.edu/R/linearModels.html,
#http://stackoverflow.com/questions/6577058/extract-regression-coefficient-values-in-r)

exponential.model$coefficients
intercept <- summary(exponential.model)$coefficients["(Intercept)", "Estimate"]
intercept2 <- exponential.model$coefficients["(Intercept)"]
xcoefficient <- summary(exponential.model)$coefficients["Time", "Estimate"]
xcoefficient2 <- exponential.model$coefficients["Time"]


#Zweites Beispiel, anwenden auf http://www.real-statistics.com/regression/exponential-regression-models/exponential-regression/
B <- structure(list(DataX = c(45, 99, 31, 57, 37, 85, 21, 64, 17, 41, 103), 
                    DataY = c(33, 72, 19, 27, 23, 62, 24, 32, 18, 36, 76)), 
               .Names = c("DataX", "DataY"), 
               
               class = "data.frame")
attach(B)
names(B)
exponential3.model <- lm(log(DataY)~ DataX)
summary(exponential3.model)

#Allgemein für Memucho
C <- structure(list(TimePassed = c(0, 1, 2, 4, 6, 8, 9, 10, 11, 12, 13, 14, 15, 
                                   16, 18, 19, 20, 21, 22, 24, 25, 26, 27), 
                    ProportionCorrect = c(71.6, 68.1, 62.9, 45.5, 41.9, 46.3, 34.1, 38.2, 41.7, 24.7, 41.5, 36.6, 
                                          19.6, 22.8, 29.6, 23.5, 15.3, 13.4, 26.8, 9.8, 18.8, 25.9, 19.3)), 
               .Names = c("TimePassed", "ProportionCorrect"), 
               
               class = "data.frame")
attach(C)
names(C)
exponential4.model <- lm(log(ProportionCorrect)~ TimePassed)
summary(exponential4.model)
regression_function = function(x){exp(exponential4.model$coefficients["(Intercept)"])*(exp(1))^(exponential4.model$coefficients["TimePassed"]*x)}
plot(regression_function, 0,30, col="green", add=TRUE)
exponential4.model$coefficients["(Intercept)"]
