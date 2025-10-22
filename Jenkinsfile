pipeline {
    agent any
    environment {
        COMPOSE_PROJECT_NAME = 'swiftapi'
        DOCKER_HOST = 'unix:///var/run/docker.sock'
    }
    
    stages {
        stage('Checkout') {
            steps {
                git branch: 'main',
                credentialsId: 'GitHub-pat',     
                url: 'https://github.com/SelfHostedApps/swiftapi.git'
            }
        }
        stage('Build and Deploy Docker') {
            when {
                branch 'main'
            }
            steps {
                echo "Building and deploying Docker containers..."
                
                // Stop and remove any existing containers
                sh '''
                    echo "[1/3] Stopping existing containers..."
                    docker compose down || true
                '''

                // Build new images and start containers
                sh '''
                    echo "[2/3] Building and starting new containers..."
                    docker compose up --build -d --remove-orphans
                '''

                // Optional: Clean up old images
                sh '''
                    echo "[3/3] Cleaning up unused images..."
                    docker image prune -f
                '''
            }
        }
    }

    post {
        success {
            echo " Deployment succeeded!"
        }
        failure {
            echo " Deployment failed."
        }
    }
}
