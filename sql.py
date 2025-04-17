import sqlite3

def get_user(username):
    # WARNING: This is intentionally vulnerable to SQL injection!
    # Never use string formatting or concatenation with SQL queries in real code!
    conn = sqlite3.connect('example.db')
    cursor = conn.cursor()
    
    # Vulnerable query - DO NOT USE IN PRODUCTION!
    query = f"SELECT * FROM users WHERE username = '{username}'"
    cursor.execute(query)
    
    result = cursor.fetchone()
    conn.close()
    return result

# Example usage (demonstrates vulnerability):
# get_user("alice")  # Normal usage
# get_user("' OR '1'='1")  # SQL injection attack that returns all users