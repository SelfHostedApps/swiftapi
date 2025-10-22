pipeline {
    agent any
    environment {
        COMPOSE_PROJECT_NAME = 'swiftapi'
        DOCKER_HOST = 'unix:///var/run/docker.sock'
    }
    
    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/SelfHostedApps/swiftapi.git'
            }
        }
        stage('Deploy') {
            when {
                branch 'main'   
            }
            steps {
                echo " Deploying ..."
                sh "/opt/deployments/scripts/deploy.sh"
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
