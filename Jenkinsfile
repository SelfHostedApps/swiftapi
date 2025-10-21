pipeline {
    agent any

    stages {
        stage('Deploy') {
            when {
                branch 'main'   
            }
            steps {
                echo " Deploying ${env.JOB_NAME}..."
                sh "/opt/deployments/scripts/deploy_${env.JOB_BASE_NAME}.sh"
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
