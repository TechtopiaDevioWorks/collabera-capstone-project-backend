#!/bin/bash
mysql -BNe 'show databases' | \
egrep -v '^(information_schema|performance_schema)$' | \
while read DB
do 
    mysql -BNe "SELECT CONCAT(\"ALTER DEFINER=\`root\`@\`localhost\` SQL SECURITY INVOKER VIEW \",table_name,\" AS \", view_definition,\";\") FROM information_schema.views WHERE table_schema=\"$DB\"" | \
    mysql $DB
done