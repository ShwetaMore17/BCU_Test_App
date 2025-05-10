from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from webdriver_manager.chrome import ChromeDriverManager
import time

# Initialize the driver
def start_driver():
    options = Options()
    options.add_argument("--ignore-certificate-errors")
    options.add_argument("--allow-insecure-localhost")
    options.add_argument("--start-maximized")
    
    service = Service(ChromeDriverManager().install())
    driver = webdriver.Chrome(service=service, options=options)
    return driver

# Test Case 1: Verify if the table has correct columns (ID, Name, Description, Code, Start Date, End Date)
def test_case_3799_1(driver):
    print("Running Test Case 1: Verify columns on the courses page")
    driver.get("https://localhost:7092/courses")
    time.sleep(2)  
    
    expected_headers = ["ID", "Name", "Description", "Code", "Start Date", "End Date"]
    headers = [th.text.strip() for th in driver.find_elements(By.CSS_SELECTOR, "table thead th")]
    
    if all(header in headers for header in expected_headers):
        print("Test Case 1 Passed")
        return True
    else:
        print(f"Test Case 1 Failed. Found headers: {headers}")
        return False

# Test Case 2: Check if the pagination displays only 100 courses per page 
def test_case_3799_2(driver):
    print("Running Test Case 2: Verify pagination shows 100 courses per page")
    driver.get("https://localhost:7092/courses")
    time.sleep(2)
    
    rows = driver.find_elements(By.CSS_SELECTOR, "table tbody tr")
    if len(rows) == 100:
        print("Test Case 2 Passed: Pagination shows 100 records per page.")
        return True
    else:
        print(f"Test Case 2 Failed: Found {len(rows)} records.")
        return False

# Test Case 3: Check if clicking "Next" works to navigate to the next page
def test_case_3799_3(driver):
    print("Running Test Case 3: Verify pagination navigation to the next page")
    driver.get("https://localhost:7092/courses")
    time.sleep(2)
    
    try:
        next_button = driver.find_element(By.CSS_SELECTOR, "button[aria-label='Next Page']")
        next_button.click()
        time.sleep(2)  # Wait for the next page to load
        rows_after = driver.find_elements(By.CSS_SELECTOR, "table tbody tr")
        print(f"Test Case 3 Passed. Found {len(rows_after)} records on the next page.")
        return True
    except Exception as e:
        print(f"Test Case 3 Failed: {e}")
        return False

# Test Case 4: Check if clicking the "View" link for a course navigates to the correct course detail page
def test_case_3799_4(driver):
    print("Running Test Case 4: Verify clicking a course name/link navigates to the correct course detail page")
    driver.get("https://localhost:7092/courses")
    time.sleep(2)
    
    try:
        view_links = driver.find_elements(By.LINK_TEXT, "View")
        if len(view_links) > 0:
            first_view_link = view_links[0]
            expected_course_id = first_view_link.get_attribute("href").split("/")[-1]
            first_view_link.click()
            time.sleep(2) 
            
            current_url = driver.current_url
            if f"/course/{expected_course_id}" in current_url:
                print(f"Test Case 4 Passed. Navigated to {current_url}")
                return True
            else:
                print(f"Test Case 4 Failed. Expected URL: /course/{expected_course_id}, Found: {current_url}")
                return False
        else:
            print("Test Case 4 Failed: No 'View' links found.")
            return False
    except Exception as e:
        print(f"Test Case 4 Failed: {e}")
        return False

# Run the tests and keep track of the pass/fail count
def run_tests():
    driver = start_driver()
    
    passed = 0
    failed = 0
    
    try:
        # Run Test Case 1
        if test_case_3799_1(driver):
            passed += 1
        else:
            failed += 1
        
        # Run Test Case 2 (if Test Case 1 passed, proceed to the next)
        if test_case_3799_2(driver):
            passed += 1
        else:
            failed += 1
        
        # Run Test Case 3 (if Test Case 2 passed, proceed to the next)
        if test_case_3799_3(driver):
            passed += 1
        else:
            failed += 1
        
        # Run Test Case 4 (if Test Case 3 passed, proceed to the next)
        if test_case_3799_4(driver):
            passed += 1
        else:
            failed += 1
        
    finally:
        driver.quit()
    
    # Print the final results
    print("\nTest Execution Complete!")
    print(f"Total Tests Run: 4")
    print(f"Tests Passed: {passed}")
    print(f"Tests Failed: {failed}")

# Run the tests
if __name__ == "__main__":
    run_tests()