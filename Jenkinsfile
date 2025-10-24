pipeline {
    agent any
    environment {
        COMPOSE_PROJECT_NAME = 'swiftapi'
        DOCKER_HOST = 'unix:///var/run/docker.sock'
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
                        sh'''
                            dotnet test \
                              --logger "trx;LogFileName=test_results.trx" \
                              --results-directory "./TestResults" \
                        '''
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
                  junit 'test_temp/TestResults/*.trx'
  
                  sh 'rm -rf test_temp/TestResults'
                }
                success {
                    echo "\t>> Success, test past with no errors"
                    script {
                        env.TEST_PASSED = 'true';
                    }
                    //emailext {

                    //}
                }
                failure {
                    echo "\t Failure, some test failed"
                    script {
                        env.TEST_PASSED = 'false';
                    }
                    //emailext{

                    //} 
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
                    sh 'docker compose down'
                    sh 'docker compose --build -d'
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

