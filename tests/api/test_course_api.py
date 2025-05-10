import requests
import pytest
from datetime import datetime

BASE_URL = "https://localhost:7437/course"
VERIFY_SSL = False

def get_response(endpoint):
    return requests.get(endpoint, verify=VERIFY_SSL)

def test_sort_option_name():
    response = get_response(f"{BASE_URL}/page/1?sortOptions=name")
    assert response.status_code == 400

def test_sort_by_start_date():
    response = get_response(f"{BASE_URL}/page/1?sortOptions=3")
    assert response.status_code == 200
    items = response.json()["items"]
    start_dates = [datetime.fromisoformat(course["startDate"]) for course in items]
    assert start_dates == sorted(start_dates)

def test_sort_by_end_date():
    response = get_response(f"{BASE_URL}/page/1?sortOptions=4")
    assert response.status_code == 200
    items = response.json()["items"]
    end_dates = [datetime.fromisoformat(course["endDate"]) for course in items]
    assert end_dates == sorted(end_dates)

def test_default_sorting():
    response = get_response(f"{BASE_URL}/page/1")
    assert response.status_code == 200

def test_all_expected_fields_are_present():
    response = get_response(f"{BASE_URL}/page/1")
    assert response.status_code == 200
    for course in response.json()["items"]:
        assert all(field in course for field in ["id", "name", "description", "code", "startDate", "endDate"])

def test_invalid_high_page_numbert():
    response = get_response(f"{BASE_URL}/page/99999999")
    assert response.status_code in [404, 200]
    if response.status_code == 200:
        assert response.json()["items"] == []

def test_negative_page_number():
    response = get_response(f"{BASE_URL}/page/-1")
    assert response.status_code == 500

def test_invalid_sort_option():
    response = get_response(f"{BASE_URL}/page/1?sortOptions=foo")
    assert response.status_code == 400

def test_missing_page_number():
    response = get_response(f"{BASE_URL}/page")
    assert response.status_code == 400

def test_total_items_field_matches_expected_count():
    response = get_response(f"{BASE_URL}/page/1")
    assert response.status_code == 200
    assert response.json()["totalItems"] == 5000  # Update if expected changes

def test_sort_option_with_special_characters_returns_400():
    response = get_response(f"{BASE_URL}/page/1?sortOptions=$%&")
    assert response.status_code == 400

def test_sql_injection_attempt_returns_400():
    response = get_response(f"{BASE_URL}/page/1?sortOptions=name';DELETE FROM course--")
    assert response.status_code == 400

def test_extremely_large_page_number():
    response = get_response(f"{BASE_URL}/page/999999999")
    assert response.status_code in [404, 200]
    if response.status_code == 200:
        assert response.json()["items"] == []

def test_dates_are_in_valid_iso_8601_format():
    response = get_response(f"{BASE_URL}/page/1")
    assert response.status_code == 200
    for course in response.json()["items"]:
        try:
            datetime.strptime(course["startDate"], "%Y-%m-%dT%H:%M:%S.%f")
            datetime.strptime(course["endDate"], "%Y-%m-%dT%H:%M:%S.%f")
        except ValueError:
            pytest.fail("Invalid date format in response")
