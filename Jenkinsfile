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
            echo " Deployment for ${env.JOB_NAME} succeeded!"
        }
        failure {
            echo " Deployment for ${env.JOB_NAME} failed."
        }
    }
}
