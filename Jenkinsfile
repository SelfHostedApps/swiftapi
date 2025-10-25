pipeline {
    agent any
    environment {
        CONTAINER_NAME = "${env.JOB_NAME.replaceAll('[^a-zA-Z0-9_-]', '_').toLowerCase()}"
        PODMAN = '/usr/bin/podman'
        DOTNET = '/usr/bin/dotnet'
    }
    
    stages {
//
//
//
//
        stage('fetch data') {
            steps {
            // get essential data to 
                script {
                    echo """
Step 1.1: extract webhook data 
________________________________________________________
"""
                    echo """
\twebhook data
\t______________________________________________________
\tAddressing changes of: ${env.BRANCH_NAME}
\tRepository: ${env.GIT_URL}
\tCommit: ${env.GIT_COMMIT}
\tMade by: ${env.GIT_AUTHOR_NAME}
\t______________________________________________________\n
"""            
                    echo """
Step 1.2: building test version...
--------------------------------------------------------
"""               
                    dir("test_temp"){
                      git branch: env.BRANCH_NAME,
                          credentialsId: 'GitHub-pat',
                          url: env.GIT_URL
                    }

                }
            }
            post {
                success {
                    echo "\t>> test branch pulled ${env.BRANCH_NAME} successful"
                }
                failure {
                    echo "\t>> FAILED test branch pulled ${env.BRANCH_NAME} unsuccessfull"
                }
                always {
                    echo "\t>> Stage finished at ${new Date().format("yyyy-MM-dd HH:mm:ss")}"
                }
            }

        }
//
//
//
//
        stage('run tests') {
            steps {
                script  {        
                    echo """
Step 2.1: Running test
________________________________________________________
"""
                    dir('test_temp') {
                        sh"""
                            ${DOTNET} test \
                              --logger "trx;LogFileName=test_results.trx" \
                              --results-directory "./TestResults" \
                              --logger "console;verbosity=detailed;consoleLoggerParameters=ErrorOnly"
                        """
                   }
                   echo """
Step 2.2: Test finished
________________________________________________________
"""
                }
            }
            post {
                always {
                    echo "\t>> archiving results and cleaning up test folder..."
                    script {
                        if (fileExists('test_temp/TestResults/test_results.trx')) {
                            
                            echo "\t>> Errors were found reporting them to jenkins"
                            junit 'test_temp/TestResults/*.trx'

                            sh 'rm -rf test_temp/TestResults'
                            env.TEST_PASSED = 'false'

                        } else {
                            echo "\t>> All test passed with flying colors"
                            env.TEST_PASSED = 'true'
                        } 
                    }
                }
                success {
                    echo "\t>> Success, test stage is good"

                }
                failure {
                    echo "\t>> Failure, test stage failed" 
                }     
            }
        }



        stage('rebuild true version') {
            when {
                allOf {
                    expression { env.BRANCH_NAME == 'main' }
                    expression { env.TEST_PASSED == 'true' }
                }
            }
            steps {

                git branch: env.BRANCH_NAME,
                credentialsId: 'GitHub-pat',
                url: env.GIT_URL
                script {
                    echo """
Step 3.1: Rebuilding project
________________________________________________________
"""
                    echo "\t>> rebuilding docker container..."
                    sh "${PODMAN}-compose -p ${CONTAINER_NAME} down || true"
                    sh "podman build -f Containerfile -t localhost/${CONTAINER_NAME} ."
                    sh "podman network create ${CONTAINER_NAME}_default || true"
                    sh "${PODMAN}-compose -p ${CONTAINER_NAME} up --build -d"
                }
            }

            post {
                success {
                    script {
                        echo "docker a successfully rebuild"
                    }

                }
                failure {
                    script {
                        echo"failure docker couldnt rebuild"
                    }
                }

            }
        }
    }
}

