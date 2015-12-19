# Run this script to create a new instance of LocalDB that corresponds with the default setting
# in the integration tests. SqlLocalDB must be installed and on the path (default setting) for 
# this to work.

# Alternatively, an environment variable can be set on machine to allow for a different instance
# of SQL Server (Regular, Express or LocalDB) to be used.

SqlLocalDB create MendhamIntegrationTest -s