package com.inventory.test;

import java.time.Duration;
import org.openqa.selenium.*;
import org.openqa.selenium.edge.EdgeDriver;
import org.openqa.selenium.support.ui.ExpectedConditions;
import org.openqa.selenium.support.ui.WebDriverWait;

import com.aventstack.extentreports.*;
import com.aventstack.extentreports.reporter.ExtentSparkReporter;

public class DashboardPageTest {

    public static void main(String[] args) {
     
        ExtentSparkReporter htmlReporter = new ExtentSparkReporter("DashboardTestReport.html");
        ExtentReports extent = new ExtentReports();
        extent.attachReporter(htmlReporter);
        ExtentTest test = extent.createTest("Dashboard Page Test");

        System.setProperty("webdriver.edge.driver", "C:\\edgedriver_win64\\msedgedriver.exe");
        WebDriver driver = new EdgeDriver();
        driver.manage().timeouts().implicitlyWait(Duration.ofSeconds(10));
        driver.manage().window().maximize();

        try {
            driver.get("https://localhost:5173");
            Thread.sleep(2000);
            test.pass("Opened inventory tracker homepage");

            driver.findElement(By.xpath("//*[@id='root']/div/div/div[2]/a")).click();
            Thread.sleep(2000);
            test.pass("Navigated to dashboard");

            // Add Item
            driver.findElement(By.xpath("//*[@id='root']/div/div[2]/div/div[1]/button[2]")).click();
            Thread.sleep(3000);
            driver.findElement(By.id("add-item-id")).sendKeys("30");
            Thread.sleep(2000);
            driver.findElement(By.id("add-item-name")).sendKeys("Test Item ");
            Thread.sleep(2000);
            driver.findElement(By.id("add-item-description")).sendKeys("test description");
            Thread.sleep(2000);
            driver.findElement(By.id("add-item-quantity")).sendKeys("60");
            Thread.sleep(2000);
            driver.findElement(By.id("add-item-category")).sendKeys("Test Category");
            Thread.sleep(2000);
            driver.findElement(By.id("add-item-min-stock")).sendKeys("15");
            Thread.sleep(2000);
            driver.findElement(By.id("add-button")).click();
            Thread.sleep(2000);
            test.pass("Item added successfully");

            // Edit Item
            driver.findElement(By.xpath("//*[@id=\"root\"]/div/div[2]/div/div[2]/div[5]/div/button[1]")).click();
            Thread.sleep(2000);
            WebElement quantityInput = driver.findElement(By.id("edit-item-quantity"));
            quantityInput.click();
            quantityInput.sendKeys(Keys.CONTROL + "a");
            quantityInput.sendKeys(Keys.BACK_SPACE);
            quantityInput.sendKeys("20");
            Thread.sleep(2000);
            driver.findElement(By.id("edit-update-button")).click();
            Thread.sleep(2000);
            test.pass("Item edited successfully");

            // Low Stock
            driver.findElement(By.xpath("//*[@id=\"root\"]/div/div[1]/nav/button[3]")).click();
            Thread.sleep(2000);
            driver.findElement(By.id("edit-button")).click();
            WebDriverWait wait = new WebDriverWait(driver, Duration.ofSeconds(10));
            WebElement quantityInput1 = wait.until(ExpectedConditions.visibilityOfElementLocated(By.id("newQuantityInput")));
            quantityInput1.click();
            quantityInput1.clear();
            quantityInput1.sendKeys("1");
            Thread.sleep(2000);
            driver.findElement(By.id("edit-update-button")).click();
            Thread.sleep(2000);
            test.pass("Low stock item quantity updated");

            // High Stock
            driver.findElement(By.xpath("//*[@id=\"root\"]/div/div[1]/nav/button[4]")).click();
            Thread.sleep(2000);
            driver.findElement(By.xpath("//*[@id=\"root\"]/div/div[2]/div/div[1]/div[2]/div/button")).click();
            WebElement quantityInput2 = wait.until(ExpectedConditions.visibilityOfElementLocated(By.id("newQuantityInput")));
            quantityInput2.click();
            quantityInput2.clear();
            quantityInput2.sendKeys("5");
            Thread.sleep(2000);
            driver.findElement(By.id("edit-update-button")).click();
            Thread.sleep(2000);
            test.pass("High stock item quantity updated");

        } catch (Exception e) {
            test.fail("Test Failed: " + e.getMessage());
            e.printStackTrace();
        } finally {
            driver.quit();
            extent.flush();  
        }
    }
}



