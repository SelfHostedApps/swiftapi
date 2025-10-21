pipeline {
    agent any

    stages {
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
