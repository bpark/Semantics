curl -X POST -H  "Content-Type: application/x-www-form-urlencoded" -d "type=native&Repository+ID=currencies&Repository+title=tr+currencies&Triple+indexes=spoc%2Cposc&EvaluationStrategyFactory=org.eclipse.rdf4j.query.algebra.evaluation.impl.StrictEvaluationStrategyFactory" "http://192.168.33.10:8080/rdf4j-workbench/repositories/NONE/create"

curl -X POST --data-binary @data/currency.ttl.gz -H "Content-Encoding: gzip" -H "Content-Type: text/turtle" http://192.168.33.10:8080/rdf4j-server/repositories/currencies/statements

curl -X POST -H  "Content-Type: application/x-www-form-urlencoded" -d "type=native&Repository+ID=geonames&Repository+title=cached+geonames&Triple+indexes=spoc%2Cposc&EvaluationStrategyFactory=org.eclipse.rdf4j.query.algebra.evaluation.impl.StrictEvaluationStrategyFactory" "http://192.168.33.10:8080/rdf4j-workbench/repositories/NONE/create"

curl -X POST --data-binary @data/geonames.ttl.gz -H "Content-Encoding: gzip" -H "Content-Type: text/turtle" http://192.168.33.10:8080/rdf4j-server/repositories/geonames/statements