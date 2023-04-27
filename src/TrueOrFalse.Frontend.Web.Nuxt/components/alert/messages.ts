export const messages: any = {
    success: {
        category: {
            publish: "Dein Thema wurde erfolgreich veröffentlicht.",
            setToPrivate: "Das Thema wurde erfolgreich auf 'Privat' gesetzt.",
            unlinked: "Die Verknüpfung wurde erfolgreich gelöst.",
            addedToPersonalWiki: "Das Thema wurde erfolgreich zu deinem Wiki hinzugefügt.",
            saveImage: "Das neue Themenbild wurde gespeichert"
        },
        question: {
            created: "Deine Frage wurde erfolgreich erstellt.",
            saved: "Deine Frage wurde erfolgreich gespeichert.",
            delete: "Deine Frage wurde erfolgreich gelöscht."
        },
        user: {
            profileUpdate: "Deine neuen Profildaten wurde erfolgreich gespeichert",
            passwordChanged: "Dein Passwort wurde erfolgreich geändert.",
            passwordReset: "Dein Passwort wurde zurückgesetzt."
        }
    },
    error: {
        subscription: {
            cantAddKnowledge: "Du kannst in der kostenlosen Version kein Wunschwissen mehr hinzufügen, ein Abonnement entfernt diese Funktionsbeschränkung.",
            cantSavePrivateQuestion: "Du kannst in der kostenlosen Version keine privaten Fragen mehr hinzufügen, ein Abonnement entfernt diese Funktionsbeschränkung",
            cantSavePrivateTopic: "Du kannst in der kostenlosen Version keine privaten Topics mehr hinzufügen, ein Abonnement entfernt diese Funktionsbeschränkung"
        },
        category: {
            parentIsPrivate: "Veröffentlichung ist nicht möglich. Das übergeordnete Thema ist privat.",
            publicChildCategories: "Dieses Thema hat öffentliche untergeordnete Themen.",
            publicQuestions: "Dieses Thema hat öffentliche Fragen.",
            notLastChild: "Dieses Thema kann nicht gelöscht werden, da weitere Themen untergeordnet sind. Bitte entferne alle Unterthemen und versuche es erneut.",
            noRemainingParents: "Die Verknüpfung des Themas kann nicht gelöst werden. Das Thema muss mindestens einem Oberthema zugeordnet sein.",
            parentIsRoot: "Unter 'Alle Themem', darfst du nur private Themen neu hinzufügen",
            loopLink: "Man kann keine Themen sich selber unterordnen",
            isAlreadyLinkedAsChild: "Das Thema ist schon untergeordnet.",
            isLinkedInNonWuwi: "Du hast das Thema außerhalb deines Wunschwissens schon untergeordnet, bitte stelle: 'Zeige nur dein Wunschwissen' aus und füge die Kategorie deinem Wunschwissen hinzu. ",
            childIsParent: "Übergeordnete Themen können nicht untergeordnet werden.",
            nameIsTaken: " ist bereits vergeben, bitte wähle einen anderen Namen!",
            nameIsForbidden: " ist verboten, bitte wähle einen anderen Namen!",
            rootCategoryMustBePublic: "Das Root Thema kann nicht auf privat gesetzt werden.",
            missingRights: "Dir fehlen die notwendigen Rechte.",
            tooPopular: "Dieses Thema ist zu oft im Wunschwissen anderer User",
            saveImage: "Das Bild konnte nicht gespeichert werden."
        },
        question: {
            missingText: "Der Fragetext fehlt.",
            missingAnswer: "Die Antwort zur Frage fehlt.",
            save: "Deine Frage konnte nicht gespeichert werden.",
            creation: "Deine Frage konnte nicht erstellt werden.",
            isInWuwi: (count: number | string) =>
                `Die Frage kann nicht gelöscht werden, sie ist ${count}x Teil des Wunschwissens anderer Nutzer. Bitte melde dich bei uns, wenn du meinst, die Frage sollte dennoch gelöscht werden.`,
            rights: "Dir fehlt die Berechtigung dazu.",
            errorOnDelete: "Es ist ein Fehler aufgetreten! Möglicherweise sind Referenzen auf die Frage (Lernsitzungen, Termine, Wunschwissen-Einträge...) teilweise gelöscht."
        },
        user: {
            emailInUse: "Die Email-Adresse ist bereits in Verwendung.",
            userNameInUse: "Dieser Benutzername ist bereits vergeben.",
            passwordIsWrong: "Falsches Passwort. Gib das Passwort erneut ein.",
            samePassword: "Das neue Passwort entspricht dem alten Passwort. Bitte gebe ein neues Passwort ein.",
            passwordNotCorrectlyRepeated: "Das wiederholte Passwort gleicht nicht deiner neuen Passworteingabe.",
            inputError: "Bitte überprüfe deine Eingaben."
        } as { [key: string]: string },
        default: "Leider ist ein unerwarteter Fehler aufgetreten, wiederhole den Vorgang zu einem späteren Zeitpunkt durch.",
        image: {
            tooBig: "Das Bild ist zu groß. Die Dateigröße darf maximal 1MB betragen."
        },
        learningSession: {
            noQuestionsAvailableWithCurrentConfig: 'Für diese Einstellungen sind keine Fragen verfügbar. Bitte ändere den Wissensstand oder wähle alle Fragen aus.'
        },
    },
    info: {
        category: {},
        question: {
            newQuestionNotInFilter: '<b>Achtung: Die Frage wird dir nach dem Erstellen nicht angezeigt,</b> da die gewählten Optionen nicht mit den Filtereinstellungen übereinstimmen, Passe den lernfilter an, um die Frage anzuzeigen.'
        },
        googleLogin: '<p>Beim Login mit Google werden Daten mit den Servern von Google ausgetauscht. Dies geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer <a href="/Impressum"> Datenschutzerklärung</a>.</p>',
        facebookLogin: '<p>Beim Login mit Facebook werden Daten mit den Servern von Facebook ausgetauscht. Dies geschieht nach erfolgreicher Anmeldung / Registrierung auch bei folgenden Besuchen. Mehr in unserer <a href="/Impressum"> Datenschutzerklärung</a>.</p>',
        questionNotInFilter: 'Die Frage kann mit deinem Fragefilter nicht angezeigt werden.',
        passwordResetRequested: (email: string) => `Sollte das Konto in unserem System vorhanden sein, haben wir eine E-Mail mit einem Link zum Zurücksetzen des Passwortes an ${email} geschickt.`
    },
}