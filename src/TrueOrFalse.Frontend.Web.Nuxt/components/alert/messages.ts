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
            passwordReset: "Dein Passwort wurde zurückgesetzt.",
            passwordVerificationMailSent: "Verifizierungs-E-Mail gesendet! Bitte überprüfe Deinen Posteingang."
        }
    },
    error: {
        subscription: {
            cantAddKnowledge: "Du kannst in der kostenlosen Version kein Wunschwissen mehr hinzufügen. Schließe eine Plus-Mitgliedschaft ab, um unbegrenztes Wunschwissen zu erhalten.",
            cantSavePrivateQuestion: "Du kannst in der kostenlosen Version keine privaten Fragen mehr hinzufügen. Schließe eine Plus-Mitgliedschaft ab, um unbegrenzt private Fragen zu erstellen.",
            cantSavePrivateTopic: "Du kannst in der kostenlosen Version keine privaten Themen mehr hinzufügen. Schließe eine Plus-Mitgliedschaft ab, um unbegrenzt private Themen zu erstellen."
        },
        category: {
            parentIsPrivate: "Veröffentlichung ist nicht möglich. Das übergeordnete Thema ist privat.",
            publicChildCategories: "Dieses Thema hat öffentliche untergeordnete Themen.",
            publicQuestions: "Dieses Thema hat öffentliche Fragen.",
            notLastChild: "Dieses Thema kann nicht gelöscht werden, da weitere Themen untergeordnet sind. Bitte entferne alle Unterthemen und versuche es erneut.",
            noRemainingParents: "Die Verknüpfung des Themas kann nicht gelöst werden. Das Thema muss mindestens einem öffentlichen Oberthema zugeordnet sein.",
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
            saveImageError: "Das Bild konnte nicht gespeichert werden."
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
            notLoggedIn: "Bitte logge dich ein.",
            emailInUse: "Die Email-Adresse ist bereits in Verwendung.",
            userNameInUse: "Dieser Benutzername ist bereits vergeben.",
            passwordIsWrong: "Falsches Passwort. Gib das Passwort erneut ein.",
            samePassword: "Das neue Passwort entspricht dem alten Passwort. Bitte gebe ein neues Passwort ein.",
            passwordNotCorrectlyRepeated: "Das wiederholte Passwort gleicht nicht deiner neuen Passworteingabe.",
            inputError: "Bitte überprüfe deine Eingaben.",
            passwordResetTokenIsInvalid: "Der Link ist leider ungültig. Wenn du Probleme hast, schreibe uns einfach eine E-Mail an team@memucho.de.",
            passwordResetTokenIsExpired: "Der Link ist abgelaufen.",
            doesNotExist: "Der angegebene Nutzern wurde nicht gefunden. Bitte überprüfen Sie, ob Sie den korrekten Anmeldedaten eingegeben haben.",
            invalidFBToken: "Hey! Sieht so aus, als wäre das Facebook-Token, das Du eingegeben hast, nicht richtig. Probier's nochmal mit einer neuen Anmeldung über Facebook. Wenn's immer noch nicht klappt, meld Dich einfach bei uns. Wir helfen Dir gerne weiter! 👍",
            emailIsInvalid: (email: string) => `${email} ist keine gültige E-Mail-Adresse.`,
            passwordTooShort: "Das Passwort sollte mindestens 5 Zeichen lang sein.",
            loginFailed: "Du konntest nicht eingeloggt werden. Bitte überprüfe deine E-Mail-Adresse und das Passwort."
        },
        default: "Leider ist ein unerwarteter Fehler aufgetreten. Wiederhole den Vorgang bitte zu einem späteren Zeitpunkt.",
        image: {
            tooBig: "Das Bild ist zu groß. Die Dateigröße darf maximal 1MB betragen."
        },
        learningSession: {
            noQuestionsAvailableWithCurrentConfig: 'Für diese Einstellungen sind keine Fragen verfügbar. Bitte ändere den Wissensstand oder wähle alle Fragen aus.',
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
        passwordResetRequested: (email: string) => `Sollte das Konto in unserem System vorhanden sein, haben wir eine E-Mail mit einem Link zum Zurücksetzen des Passwortes an ${email} geschickt.`,
        joinNow: 'Bereit für unbegrenzten Zugriff? - Werde Plus-Mitglied!'
    },
    getByCompositeKey(messageKey: string): string | undefined {
        const keyParts = messageKey?.split('_');
        let currentLevel = messages;
        for (const part of (keyParts ?? ["WillReturnUndefined"])) {
            if (currentLevel.hasOwnProperty(part)) {
                currentLevel = currentLevel[part];
            } else {
                console.error(`Unknown key: ${messageKey}`)
                const { $logger } = useNuxtApp()
                const err = new Error()
                $logger.error(`Unknown key: ${messageKey}`, [{ stack: err.stack }])
                return undefined; // Key part not found in the messages structure
            }
        }
        // console.log(`MessageKey: ${messageKey}, Message: ${currentLevel}`)
        return currentLevel as string;
    }
}