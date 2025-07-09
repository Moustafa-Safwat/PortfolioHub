up-dev:
	docker compose -f docker-compose.yml -f docker-compose.dev.yml --env-file .env.dev up --build -d

up-pre:
	docker compose -f docker-compose.yml -f docker-compose.pre.yml --env-file .env.pre up --build -d

up-prod:
	docker compose -f docker-compose.yml -f docker-compose.prod.yml --env-file .env.prod up --build -d